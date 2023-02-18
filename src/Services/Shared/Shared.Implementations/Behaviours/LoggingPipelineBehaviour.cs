using MediatR.Pipeline;
using Shared.Implementations.Logging;

namespace Shared.Implementations.Behaviours;

public class LoggingPipelineBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILoggerManager<LoggingPipelineBehaviour<TRequest>> _logger;

    public LoggingPipelineBehaviour(ILoggerManager<LoggingPipelineBehaviour<TRequest>> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).Name;
        _logger.LogInformation(null, $"TeamTitan Request: {name} - {request}");

        return Task.CompletedTask;
    }
}