using Management.API.Configurations;
using Management.Application;
using Management.Infrastructure.Configurations;
using Serilog;
using Shared.Implementations;
using Shared.Implementations.Logging;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

var commonFolder = Path.Combine(env.ContentRootPath, "..\\..", "Shared");

builder.Configuration.AddJsonFile(Path.Combine(commonFolder, "SharedSettings.json"), optional: true)
    .AddJsonFile("SharedSettings.json", optional: true)
    .AddJsonFile($"SharedSettings.{env.EnvironmentName}.json", optional: true)
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).AddEnvironmentVariables();
 

// Add services to the container.

builder.GetDependencyInjectionInfrastructure().GetDependencyInjectionApplication().GetOptions(); 

builder.RegisterSharedImplementations(typeof(AssemblyManagementApplicationReference).Assembly,
    _ => _["ConnectionStrings:DefaultManagementConnection"], types: new[]
    {
        typeof(AssemblyManagementApplicationReference),
        typeof(AssemblySharedImplementationsReference)
    }); 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.RegisterJwtBearer();

builder.Host.UseSerilog((ctx, lc) => lc.LogConfigurationService());

builder.GetSwaggerConfiguration();

var app = builder.Build();
app.SubscribeEvents();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
