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
        var connection = builder.Configuration["ConnectionStrings:DefaultCalculatorConnection"];

        builder.Services.AddScoped<IAccountRepository, AccountRepository>(_ =>
            new AccountRepository(connection,
                _.GetService<ILoggerManager<AccountRepository>>()!));
        
        builder.Services.AddScoped<IProductRepository, ProductRepository>(_ =>
            new ProductRepository(connection,
                _.GetService<ILoggerManager<ProductRepository>>()!));

        return builder;
    }
}