using FluentValidation;
using MediatR;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Logging;

namespace Shared.Implementations.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILoggerManager<ValidationPipelineBehaviour<TRequest, TResponse>> _loggerManager;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators, ILoggerManager<ValidationPipelineBehaviour<TRequest, TResponse>> loggerManager)
    {
        this._validators = validators ?? throw new ArgumentNullException(nameof(validators));
        this._loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var commandType = request.GetType().FullName;
        this._loggerManager.LogInformation($"----- Validating command {commandType} --------");

        var errorList = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errorList.Any())
        {
            this._loggerManager.LogWarning(new
            {
                Message = "Validation errors",
                Command = commandType,
                Errors = errorList
            });

            throw new ValidatorException(errorList);
        }


        return await next();
    }
}