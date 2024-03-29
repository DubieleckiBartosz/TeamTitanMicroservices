using Calculator.API.Configurations;
using Calculator.Application;
using Calculator.Application.Contracts.Repositories;
using Calculator.Application.Projections;
using Calculator.Infrastructure.Configurations;
using Serilog;
using Shared.Implementations;
using Shared.Implementations.Core;
using Shared.Implementations.Logging;
using Shared.Implementations.Projection;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).AddEnvironmentVariables();

// Add services to the container.

builder.GetDependencyInjectionInfrastructure();

builder.RegisterSharedImplementations(typeof(AssemblyCalculatorApplicationReference).Assembly, projectionFunc: _ =>
    new List<IProjection>
    {
        new AccountProjection(_.GetService<IAccountRepository>()!),
        new ProductProjection(_.GetService<IProductRepository>()!)
    }, withBackgroundDb: true, types: new[]
{
    typeof(AssemblyCalculatorApplicationReference), typeof(AssemblySharedImplementationsReference)
}); 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 

builder.RegisterJwtBearer();

builder.Host.UseSerilog((ctx, lc) => lc.LogConfigurationService());
builder.GetSwaggerConfiguration();

var app = builder.Build();
app.SubscribeEvents();
app.RegisterBackgroundDashboard();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler(_ => 0, ErrorHandlingMiddleware.GetBaseErrorResponse);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }
