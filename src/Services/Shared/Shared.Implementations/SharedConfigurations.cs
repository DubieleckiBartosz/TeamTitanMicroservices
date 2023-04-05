using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Shared.Implementations.Decorators;
using System.Reflection;
using MediatR;
using Shared.Implementations.Communications.Email;
using Shared.Implementations.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Behaviours;
using Shared.Implementations.EventStore;
using Shared.Implementations.Mongo;
using Shared.Implementations.Outbox.MongoOutbox;
using Shared.Implementations.Outbox;
using Shared.Implementations.Projection;
using Shared.Implementations.RabbitMQ;
using Shared.Implementations.Services;
using System.Text;
using Microsoft.AspNetCore.Http;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Snapshot;
using Hangfire;
using Shared.Implementations.Background;
using Shared.Implementations.Dapper;
using Shared.Implementations.ProcessDispatcher;

namespace Shared.Implementations;

public static class SharedConfigurations
{
    public static WebApplicationBuilder RegisterSharedImplementations(this WebApplicationBuilder builder,
        Assembly? mapperAssembly = null, Func<IConfiguration, string>? connectionFunc = null,
        Func<IServiceProvider, List<IProjection>>? projectionFunc = null, string? backgroundDbConnection = null,
        bool withBackgroundDb = false,
        params Type[] types)
    {
        //MEDIATOR
        var services = builder.Services;
        var config = builder.Configuration;
        services.AddTransient<IDomainDecorator, MediatRDecorator>();
        var assemblies = types.Select(type => type.GetTypeInfo().Assembly);

        foreach (var assembly in assemblies)
        {
            services.AddMediatR(assembly);
        }

        //MAPPER
        if (mapperAssembly != null)
        {
            services.AddAutoMapper(mapperAssembly);
        }

        //TRANSACTIONS
        if (connectionFunc != null)
        {
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            services.AddScoped<ITransaction, TransactionSupervisor>(_ =>
                new TransactionSupervisor(_.GetService<ILoggerManager<TransactionSupervisor>>()!, connectionFunc.Invoke(config)));
        }

        //DEPENDENCY INJECTION 

        //Accessories
        services.GetAccessoriesDependencyInjection();

        //EVENT
        services.AddScoped<ICommandBus, CommandBus>();
        services.AddScoped<IQueryBus, QueryBus>();
        services.AddScoped<IStore, Store>();
        services.AddScoped<IEventBus, EventBus>();
        services.AddScoped<IEventStore, EventStore.EventStore>(_ =>
            new EventStore.EventStore(_.GetService<IStore>() ?? throw new ArgumentNullException(nameof(IStore)),
                projectionFunc?.Invoke(_)));

        services.AddScoped<ISnapshotStore, SnapshotStore>();

        services.AddScoped<IOutboxListener, OutboxListener>();
        services.AddScoped<IOutboxStore, OutboxStore>();
        services.AddSingleton<IRabbitBase, RabbitBase>();
        services.AddScoped<IRabbitEventListener, RabbitEventListener>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
         
        //PIPELINES
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));


        //OPTIONS
        services.Configure<StoreOptions>(config.GetSection(nameof(StoreOptions)));
        services.Configure<MongoOutboxOptions>(config.GetSection(nameof(MongoOutboxOptions)));
        services.Configure<RabbitOptions>(config.GetSection(nameof(RabbitOptions)));

        //HOSTED service
        services.AddHostedService<OutboxProcessor>();

        //MONGO 
        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

        var options = new StoreOptions();
        config.GetSection(nameof(StoreOptions)).Bind(options);

        var connectionString = options.ConnectionString;
        var database = options.DatabaseName;

        services.AddSingleton<MongoContext>(_ => new MongoContext(connectionString, database));


        //HANGFIRE
        if (withBackgroundDb)
        {
            builder.GetBackgroundConnectionSettings(backgroundDbConnection ??
                                                    builder.Configuration["ConnectionStrings:HangfireConnection"]);
        } 

        return builder;
    } 

    public static IServiceCollection GetAccessoriesDependencyInjection(this IServiceCollection services)
    {        
        //USER
        services.AddHttpContextAccessor();
        services.AddTransient<ICurrentUser, CurrentUser>();
        
        //EMAIL
        services.AddScoped<IEmailRepository, EmailRepository>();

        //LOGGER
        services.AddSingleton(typeof(ILoggerManager<>), typeof(LoggerManager<>));

        return services;
    } 

    public static WebApplication UseSubscribeAllEvents(this WebApplication app, Assembly assembly)
    {
        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            var attribute = Attribute.GetCustomAttribute(type, typeof(EventQueueAttribute));
            if (attribute == null)
            {
                continue;
            }

            var attr = (EventQueueAttribute)attribute;
            var valueQueueName = attr?.QueueName;
            var valueRoutingKey = attr?.RoutingKey;

            app.UseSubscribeEvent(assembly, type, valueQueueName, valueRoutingKey);

        }

        return app;
    }

    public static WebApplication UseSubscribeEvent(this WebApplication app, Assembly assembly, Type type, string? queueName = null,
        string? routingKey = null)
    {
        using var scope = app.Services.CreateScope();

        var requiredService = scope.ServiceProvider.GetRequiredService<IRabbitEventListener>();

        requiredService?.Subscribe(assembly, type, queueName, routingKey);

        return app;
    }


    public static WebApplication RegisterBackgroundDashboard(this WebApplication app)
    {
        app.UseHangfireDashboard(); 
        
        return app;
    }

    public static IServiceCollection RegisterJwtBearer(this IServiceCollection services, string issuer, string audience, string key)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
                o.Events = new JwtBearerEvents()
                {
                    //OnChallenge = async (context) =>
                    //{

                    //    if (context.AuthenticateFailure != null)
                    //    {
                    //        var error = string.IsNullOrEmpty(context.ErrorDescription) ? context.AuthenticateFailure?.Message : context.ErrorDescription;
                    //        context.HandleResponse();
                    //        context.Response.ContentType = "application/json";
                    //        context.Response.StatusCode = 401;
                    //        await context.Response.WriteAsJsonAsync(new {Message = $"401 Not authorized: {error}" });
                    //    }
                    //}

                    OnChallenge = async (context) =>
                    {
                        if (context.AuthenticateFailure != null)
                        {
                            var error = string.IsNullOrEmpty(context.ErrorDescription)
                                ? context.AuthenticateFailure?.Message
                                : context.ErrorDescription;
                            context.HandleResponse();
                            context.Response.ContentType = "application/json";
                            var statusCode = context?.AuthenticateFailure is SecurityTokenExpiredException
                                ? 403
                                : 401;
                            if (context != null)
                            {
                                context.Response.StatusCode = statusCode;
                                await context.Response.WriteAsJsonAsync(new
                                {
                                    ErrorMessage = statusCode == 403
                                        ? $"403 Expired token: {error}"
                                        : $"401 Not authorized: {error}"
                                });
                            }
                        }
                    }
                };
            });

        return services;
    }
}