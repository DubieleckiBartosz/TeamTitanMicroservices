using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Shared.Implementations.Behaviours;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
    private readonly ILoggerManager<TransactionBehavior<TRequest, TResponse>> _loggerManager;
    private readonly ITransaction _transaction;

    public TransactionBehavior(IRequestHandler<TRequest, TResponse> requestHandler,
        ILoggerManager<TransactionBehavior<TRequest, TResponse>> loggerManager, ITransaction transaction)
    {
        this._requestHandler = requestHandler ?? throw new ArgumentNullException(nameof(requestHandler));
        this._loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
        this._transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _loggerManager.LogInformation(null, "test log");
        var hasTransactionAttribute = this._requestHandler.GetType()
            .GetCustomAttributes(typeof(WithTransactionAttribute), false).Any();

        if (!hasTransactionAttribute)
        {
            return await next();
        }
        else
        {
            try
            {
                this._loggerManager.LogInformation(
                    $"The transaction will be created by {request.GetType().FullName} ------ HANDLER WITH TRANSACTION ------- ");

                await this._transaction.GetOpenOrCreateTransaction();

                var response = await next();

                var result = this._transaction.Commit();

                if (result)
                {
                    this._loggerManager.LogInformation(
                        $"Committed transaction {request.GetType().FullName} ------ COMMITTED TRANSACTION IN HANDLER ------- ");
                }

                return response;
            }
            catch (Exception ex)
            {
                this._loggerManager.LogError(new
                {
                    Message = "ERROR Handling transaction.",
                    DataException = ex,
                    Request = request,
                });

                throw;
            }
        }
    }
     
}