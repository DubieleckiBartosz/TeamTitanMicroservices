using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Titan.OcelotGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

var config = builder.Configuration;
var envName = config["Container_ENV"];

config.AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    .GetOcelotFile(envName);


var settings = new JwtSettings();
config.GetSection(nameof(JwtSettings)).Bind(settings);

var issuer = settings.Issuer!;
var audience = settings.Audience!;
var key = settings.Key!;

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddOcelot(config);

builder.Services.RegisterTokenBearer(issuer, audience, key); 

var app = builder.Build();

app.UseCors("CorsPolicy");
await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
