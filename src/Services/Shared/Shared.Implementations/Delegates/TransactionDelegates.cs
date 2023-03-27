namespace Shared.Implementations.Delegates;

public class TransactionDelegates
{
    public delegate bool CommitTransaction();
    public delegate void RollbackTransaction();
}