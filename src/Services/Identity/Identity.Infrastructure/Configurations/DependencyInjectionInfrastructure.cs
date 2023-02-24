using Identity.Application.Contracts;
using Identity.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Configurations;

public static class DependencyInjectionInfrastructure
{
    public static IServiceCollection GetDependencyInjectionInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}