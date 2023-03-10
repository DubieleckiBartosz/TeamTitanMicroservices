using Calculator.Application;
using Shared.Implementations;
using Shared.Implementations.Projection;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

var commonFolder = Path.Combine(env.ContentRootPath, "..\\..", "Shared");

builder.Configuration.AddJsonFile(Path.Combine(commonFolder, "SharedSettings.json"), optional: true)
    .AddJsonFile("SharedSettings.json", optional: true)
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).AddEnvironmentVariables();


// Add services to the container.

builder.EventStoreConfiguration(_ => new List<IProjection>()
{
    
}, typeof(AssemblyCalculatorApplicationReference), typeof(AssemblySharedImplementationsReference)).RegisterBackgroundProcess();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.SubscribeEvents();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
