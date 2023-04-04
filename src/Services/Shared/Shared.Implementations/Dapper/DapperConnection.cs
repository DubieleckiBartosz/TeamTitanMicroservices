using Polly;
using Shared.Implementations.Logging;
using Shared.Implementations.Polly;
using System.Data.SqlClient;
using System.Data;

namespace Shared.Implementations.Dapper;

public class DapperConnection<TRepository>
{
    private readonly string _connectionString;
    private readonly ILoggerManager<TRepository> _loggerManager; 
    private readonly AsyncPolicy _retryAsyncPolicyQuery;
    private readonly AsyncPolicy _retryAsyncPolicyConnection;
    public DapperConnection(string connectionString, ILoggerManager<TRepository> loggerManager)
    {
        _connectionString = connectionString;
        this._loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager)); 
        var policy = new PolicySetup();
        this._retryAsyncPolicyConnection = policy.PolicyConnectionAsync(this._loggerManager);
        this._retryAsyncPolicyQuery = policy.PolicyQueryAsync(this._loggerManager);
    }

    public async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> funcData, IDbTransaction? transaction = null)
    {
        try
        {
            SqlConnection? connection;

            if (transaction != null)
            {
                connection = transaction.Connection as SqlConnection;
                return await this._retryAsyncPolicyQuery.ExecuteAsync(async () => await funcData(connection!));
            }

            await using (connection = new SqlConnection(this._connectionString))
            {
                await this._retryAsyncPolicyConnection.ExecuteAsync(async () => await connection.OpenAsync());

                _loggerManager.LogInformation(null, message: "Connection has been opened");

                return await this._retryAsyncPolicyQuery.ExecuteAsync(async () => await funcData(connection));
            }
        }
        catch (TimeoutException ex)
        {
            throw new Exception($"Timeout sql exception: {ex}");
        }
        catch (SqlException ex)
        {
            throw new Exception($"Sql exception: {ex}");
        }
    }

}