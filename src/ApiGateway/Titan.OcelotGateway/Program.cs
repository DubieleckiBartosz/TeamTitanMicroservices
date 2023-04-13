using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware; 

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
builder.Configuration 
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    .AddJsonFile($"ocelot.{env.EnvironmentName}.json", optional: true).AddEnvironmentVariables();


var settings = new JwtSettings();
builder.Configuration.GetSection(nameof(JwtSettings)).Bind(settings);

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

builder.Services.AddOcelot(builder.Configuration);

builder.Services.RegisterTokenBearer(issuer, audience, key); 

var app = builder.Build();

app.UseCors("CorsPolicy");
await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
