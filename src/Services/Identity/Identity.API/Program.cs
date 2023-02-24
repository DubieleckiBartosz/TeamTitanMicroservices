using Identity.API.Common;
using Identity.API.Configurations;
using Serilog;
using Shared.Implementations.Core;
using Shared.Implementations.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.UseCors(config =>
{
    config
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
