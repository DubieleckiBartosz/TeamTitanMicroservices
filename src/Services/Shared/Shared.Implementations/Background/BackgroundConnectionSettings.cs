using Hangfire.SqlServer;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;

namespace Shared.Implementations.Background;

public static class BackgroundConnectionSettings
{
    public static WebApplicationBuilder GetBackgroundConnectionSettings(this WebApplicationBuilder builder, string connection)
    {   
        builder.Services.AddHangfire(_ =>
        {
            _.UseSqlServerStorage(connection,
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            _.UseSerializer();
        });

        builder.Services.AddHangfireServer();

        return builder;
    }
    private static void UseSerializer(this IGlobalConfiguration configuration)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        configuration.UseSerializerSettings(jsonSettings);
    }
}