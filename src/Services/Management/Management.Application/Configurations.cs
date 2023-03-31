using Management.Application.Contracts.Services;
using Management.Application.Options;
using Management.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Implementations;
using Shared.Implementations.Communications.Email;

namespace Management.Application;

public static class Configurations
{
    public static WebApplication SubscribeEvents(this WebApplication app)
    {
        app.UseSubscribeAllEvents(typeof(AssemblyManagementApplicationReference).Assembly);

        return app;
    } 
    
    public static WebApplicationBuilder GetDependencyInjectionApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMessageService, MessageService>();

        return builder;
    }

    public static WebApplicationBuilder GetOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions")); 
        builder.Services.Configure<LinkOptions>(builder.Configuration.GetSection("LinkOptions")); 

        return builder;
    }
}