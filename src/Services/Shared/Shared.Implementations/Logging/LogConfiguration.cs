using Serilog.Events;
using Serilog;

namespace Shared.Implementations.Logging;

public static class LogConfiguration
{
    public static void LogConfigurationService(this LoggerConfiguration loggerConfiguration)
    {
        var dateTimeNowString = $"{DateTime.Now:yyyy-MM-dd}";

        loggerConfiguration
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Logger(
                _ => _.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File($"Logs/{dateTimeNowString}-Error.log",
                        rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 100000)
            )
            .WriteTo.Logger(
                _ => _.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                    .WriteTo.File($"Logs/{dateTimeNowString}-Warning.log",
                        rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 100000)
            )
            .WriteTo.File($"Logs/{dateTimeNowString}-All.log")
            .WriteTo.Console()
                        //.WriteTo.Seq("http://localhost:5341");
            .WriteTo.Seq("http://seq");

    }
}