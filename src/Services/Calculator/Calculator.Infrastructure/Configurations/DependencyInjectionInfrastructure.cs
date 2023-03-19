using Calculator.Application.Contracts;
using Calculator.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Implementations.Logging;

namespace Calculator.Infrastructure.Configurations;

public static class DependencyInjectionInfrastructure
{
    public static WebApplicationBuilder GetDependencyInjectionInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAccountRepository, AccountRepository>(_ =>
            new AccountRepository(builder.Configuration["ConnectionStrings:DefaultCalculatorConnection"],
                _.GetService<ILoggerManager<AccountRepository>>()!));
        return builder;
    }
}