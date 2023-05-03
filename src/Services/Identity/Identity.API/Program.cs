using Identity.API.Common;
using Identity.API.Configurations;
using Serilog;
using Shared.Implementations;
using Shared.Implementations.Core;
using Shared.Implementations.Logging;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).AddEnvironmentVariables();

// Add services to the container.
builder.RegisterBackgroundConnectionSettings();
builder.Host.UseSerilog((ctx, lc) => lc.LogConfigurationService());

builder.ApiConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseLoggerHandler();
app.UseCustomExceptionHandler(ErrorMiddleware.GetStatusCode, ErrorMiddleware.GetErrorResponse);
 
app.UseHttpsRedirection(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.Services.GetService<IBackgroundService>()?.StartJobs();

app.Run();
