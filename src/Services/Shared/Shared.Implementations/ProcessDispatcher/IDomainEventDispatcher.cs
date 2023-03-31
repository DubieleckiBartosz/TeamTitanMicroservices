using Shared.Domain.Base;
using Shared.Implementations.Delegates;

namespace Shared.Implementations.ProcessDispatcher;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync<T>(T entity, TransactionDelegates.CommitTransaction? commit = null,
        TransactionDelegates.RollbackTransaction? rollback = null)
        where T : Entity;
}