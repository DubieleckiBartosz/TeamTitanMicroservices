using Calculator.Application.Contracts;
using Calculator.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Infrastructure.Configurations;

public static class DependencyInjectionInfrastructure
{
    public static WebApplicationBuilder GetDependencyInjectionInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IBonusProgramRepository, BonusProgramRepository>();
        return builder;
    }
}