using General.API.Configurations;
using General.Infrastructure.Database;
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
builder.GetDependencyInjection().GetShared();
builder.Services.RegisterGeneralDatabase(builder.Configuration["ConnectionStrings:DefaultGeneralConnection"]);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.RegisterJwtBearer();
builder.Host.UseSerilog((ctx, lc) => lc.LogConfigurationService());
builder.GetSwagger();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migration = scope.ServiceProvider
        .GetRequiredService<AutomaticMigration>();
     
    migration.RunMigration();
}

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
