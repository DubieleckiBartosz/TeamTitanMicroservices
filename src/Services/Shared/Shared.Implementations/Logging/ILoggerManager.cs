namespace Shared.Implementations.Logging;

public interface ILoggerManager<T>
{
    void LogInformation(object? obj = null, string? message = null);
    void LogWarning(object? obj = null, string? message = null);
    void LogError(object? obj = null, string? message = null);
    void LogCritical(object? obj = null, string? message = null);
}