using Polly;
using RabbitMQ.Client.Exceptions;
using System.Data.SqlClient;
using System.Net.Sockets;
using Shared.Implementations.Logging;

namespace Shared.Implementations.Polly;

public class PolicySetup
{
    private const int RetryCount = 3;
    public PolicySetup()
    {
    }

    public Policy PolicyBrokerConnection<T>(ILoggerManager<T> logger) => Policy.Handle<BrokerUnreachableException>()
        .Or<SocketException>()
        .WaitAndRetry(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
        {
            logger.LogWarning(new
            {
                Message = "Could not publish event",
                Timeout = $"{time.TotalSeconds:n1}",
                ExceptionMessage = ex.Message
            });
        });


    public AsyncPolicy PolicyConnectionAsync<T>(ILoggerManager<T> logger) => Policy
        .Handle<SqlException>()
        .Or<TimeoutException>()
        .WaitAndRetryAsync(
            3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (exception, timeSpan, retryCount, context) =>
            {
                logger?.LogError(new
                {
                    RetryAttempt = retryCount,
                    ExceptionMessage = exception?.Message,
                    Waiting = timeSpan.Seconds
                });
            }
        );


    public AsyncPolicy PolicyQueryAsync<T>(ILoggerManager<T> logger) => Policy.Handle<SqlException>()
        .WaitAndRetryAsync(
            3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (exception, timeSpan, retryCount, context) =>
            {
                logger?.LogError(new
                {
                    RetryAttempt = retryCount,
                    ExceptionMessage = exception?.Message,
                    ProcedureName = this.GetProcedure(exception),
                    Waiting = timeSpan.Seconds
                });
            }
        );


    #region Private

    private string GetProcedure(Exception exception) => exception is SqlException ex ? ex.Procedure : string.Empty;

    #endregion
}