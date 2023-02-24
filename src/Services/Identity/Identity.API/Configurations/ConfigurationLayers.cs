using Identity.Application.Configurations;
using Identity.Infrastructure.Configurations;

namespace Identity.API.Configurations;

public static class ConfigurationLayers
{
    public static IServiceCollection GetConfigurationLayers(this IServiceCollection services)
    {
        //Application
        services.GetValidators();
        services.GetDependencyInjectionApplication();

        //Infrastructure
        services.GetDependencyInjectionInfrastructure();

        return services;
    }
}