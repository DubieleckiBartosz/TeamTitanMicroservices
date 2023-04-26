namespace Titan.OcelotGateway.Extensions;

public static class OcelotConfiguration
{
    public static IConfigurationBuilder GetOcelotFile(this IConfigurationBuilder builderConfig, string? envName = null)
    { 
        builderConfig.AddJsonFile($"ocelot.{envName}.json", optional: false).AddEnvironmentVariables(); 

        return builderConfig;
    }
}