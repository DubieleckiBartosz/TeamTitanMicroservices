namespace Titan.OcelotGateway.Extensions;

public static class OcelotConfiguration
{
    public static IConfigurationBuilder GetOcelotFile(this IConfigurationBuilder builderConfig, IConfiguration configuration, string envName)
    {
        //Temporarily we only have two environments local and docker. Now that should be enough

        var ocelotEnvFileName = int.TryParse(configuration["Docker_Dev"], out int value) ? envName : "Local"; 

        builderConfig.AddJsonFile($"ocelot.{ocelotEnvFileName}.json", optional: true).AddEnvironmentVariables();


        return builderConfig;
    }
}