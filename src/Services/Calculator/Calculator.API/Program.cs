using Calculator.API.Configurations;
using Calculator.Application;
using Calculator.Application.Contracts;
using Calculator.Application.Projections;
using Calculator.Infrastructure.Configurations;
using Serilog;
using Shared.Implementations;
using Shared.Implementations.Core;
using Shared.Implementations.Logging;
using Shared.Implementations.Projection;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

var commonFolder = Path.Combine(env.ContentRootPath, "..\\..", "Shared");

builder.Configuration.AddJsonFile(Path.Combine(commonFolder, "SharedSettings.json"), optional: true)
    .AddJsonFile("SharedSettings.json", optional: true)
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).AddEnvironmentVariables();


// Add services to the container.

builder.GetDependencyInjectionInfrastructure();

builder.EventStoreConfiguration(_ => new List<IProjection>
    {
        new AccountProjection(_.GetService<IAccountRepository>()!),
        new ProductProjection(_.GetService<IProductRepository>()!)
    }, typeof(AssemblyCalculatorApplicationReference), typeof(AssemblySharedImplementationsReference))
    .RegisterBackgroundProcess().GetHangfire().GetAutoMapper(typeof(AssemblyCalculatorApplicationReference).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var issuer = builder.Configuration["JwtSettings:Issuer"];
var audience = builder.Configuration["JwtSettings:Audience"];
var key = builder.Configuration["JwtSettings:Key"];

builder.Services.RegisterJwtBearer(issuer, audience, key);

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
