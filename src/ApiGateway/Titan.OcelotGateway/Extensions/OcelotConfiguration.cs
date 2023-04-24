namespace Titan.OcelotGateway.Extensions;

public static class OcelotConfiguration
{
    public static IConfigurationBuilder GetOcelotFile(this IConfigurationBuilder builderConfig, string? envName = null)
    {
        //Temporarily we only have two environments local and docker. Now that should be enough

        envName ??= "Local";
        builderConfig.AddJsonFile($"ocelot.{envName}.json", optional: true).AddEnvironmentVariables(); 

        return builderConfig;
    }
}