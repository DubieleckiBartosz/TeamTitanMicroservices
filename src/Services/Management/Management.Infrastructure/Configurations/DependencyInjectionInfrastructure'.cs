using Management.Application.Contracts.Repositories;
using Management.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Implementations.Dapper;
using Shared.Implementations.ProcessDispatcher;

namespace Management.Infrastructure.Configurations;

public static class DependencyInjectionInfrastructure
{
    public static WebApplicationBuilder GetDependencyInjectionInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(_ =>
            new UnitOfWork(builder.Configuration["ConnectionStrings:DefaultManagementConnection"],
                _.GetService<IDomainEventDispatcher>()!, _.GetService<ITransaction>()!, _));
        return builder;
    }
}