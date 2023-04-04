using Polly;
using Shared.Implementations.Logging;
using Shared.Implementations.Polly;
using System.Data.SqlClient;
using System.Data;
using Shared.Implementations.Core.Exceptions;

namespace Shared.Implementations.Dapper;

public class TransactionSupervisor : ITransaction
{
    private readonly ILoggerManager<TransactionSupervisor> _loggerManager;
    private readonly string _connectionString;
    private readonly AsyncPolicy _retryAsyncPolicyConnection;
    private string _transactionId;

    private SqlConnection? _connection;
    private SqlTransaction? _transaction;

    public TransactionSupervisor(ILoggerManager<TransactionSupervisor> loggerManager, string dbConnection)
    {
        this._loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
        this._connectionString = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        var policy = new PolicySetup();
        this._retryAsyncPolicyConnection = policy.PolicyConnectionAsync(this._loggerManager);
    }

    public async Task<IDbTransaction?> GetOpenOrCreateTransaction()
    { 
        try
        {
            if (this._transaction != null)
            {
                return this._transaction;
            }

            this.TransactionIdGenerator();
            
            if (this._connection != null)
            {  
                this._transaction = this._connection.BeginTransaction();
                return this._transaction;
            }

            this._connection = new SqlConnection(this._connectionString);
            await this._retryAsyncPolicyConnection.ExecuteAsync(async () => await this._connection.OpenAsync());
            this._transaction = this._connection.BeginTransaction();
            return this._transaction;
        }
        catch (Exception ex)
        {
            throw new DatabaseException($"{ex?.InnerException}", "CreateTransaction method exception");
        }
    }

    public IDbTransaction? GetTransactionWhenExist()
    {
        return this._transaction;
    }

    public void Rollback()
    {
        try
        {
            if (_transaction != null)
            {
                this._loggerManager.LogInformation(null, message: "Rollback start.");
                this._transaction?.Rollback();
                this._loggerManager.LogInformation(null, message: "Rollback OK.");
            }
        }
        catch
        {
            this._loggerManager.LogError("Rollback failed.");
            throw;
        }
        finally
        {
            if (this._transaction != null)
            {
                this._transaction.Dispose();
                this._transaction?.Connection?.Dispose();
                this._transaction = null;
            }
        }
    }

    public bool Commit()
    {
        try
        {
            if (this._transaction == null)
            {
                this._loggerManager.LogInformation("Transaction is null.");
                return false;
            }

            this._transaction.Commit();
            this._loggerManager.LogInformation(
                $"Transaction committed successfully ------ TRANSACTION ------- : {this._transactionId}");
            return true;
        }
        catch
        {
            _loggerManager.LogError("Commit failed.");
            this.Rollback();
            throw;
        }
        finally
        {
            if (this._transaction != null)
            {
                this._transaction.Dispose();
                this._transaction?.Connection?.Dispose();
                this._transaction = null;
            }
        }
    }

    private void TransactionIdGenerator()
    {
        this._transactionId = Guid.NewGuid().ToString();
        this._loggerManager.LogInformation(null, $"New transaction identifier created: {this._transactionId}");
    }
}