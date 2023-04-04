using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Shared.Implementations.Logging;

public class LoggerManager<T> : ILoggerManager<T>
{
    private readonly ILogger<T> _logger;

    public LoggerManager(ILogger<T> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void LogInformation(object? obj = null, string? message = null) =>
        _logger.LogInformation(this.GetMessage(obj, message));

    public void LogWarning(object? obj = null, string? message = null) => _logger.LogWarning(this.GetMessage(obj, message));

    public void LogError(object? obj = null, string? message = null) => _logger.LogError(this.GetMessage(obj, message));

    public void LogCritical(object? obj = null, string? message = null) =>
        _logger.LogCritical(this.GetMessage(obj, message));

    private string GetMessage(object? obj, string? message) => obj == null && message == null
        ? throw new ArgumentNullException(nameof(message))
        : message ?? JsonConvert.SerializeObject(obj);
}