using System.Data;

namespace Shared.Implementations.Dapper;

public interface ITransaction
{
    IDbTransaction? GetTransactionWhenExist();
    Task<IDbTransaction?> GetOpenOrCreateTransaction();
    bool Commit();
    void Rollback();
}