using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Implementations.Delegates;
using Shared.Implementations.Logging;

namespace Shared.Implementations.ProcessDispatcher;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IDomainDecorator _decorator;
    private readonly ILoggerManager<DomainEventDispatcher> _loggerManager;

    public DomainEventDispatcher(IDomainDecorator decorator, ILoggerManager<DomainEventDispatcher> loggerManager)
    {
        this._decorator = decorator ?? throw new ArgumentNullException(nameof(decorator));
        this._loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
    }

    public async Task DispatchEventsAsync<T>(T entity, TransactionDelegates.CommitTransaction? commit = null,
        TransactionDelegates.RollbackTransaction? rollback = null)
        where T : Entity
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            var events = entity?.Events;
            if (events != null && events.Any())
            {
                this._loggerManager.LogInformation(null, message: "Sending events to handlers has started.");
                foreach (var @event in events)
                {
                    await this._decorator.Publish(@event);
                }

                this._loggerManager.LogInformation(null, message: "All events were sent positively.");

                entity!.ClearEvents();
            }

            commit?.Invoke();
        }
        catch (Exception ex)
        {
            this._loggerManager.LogInformation(new
            {
                Messsage = "Error sending events to handlers.",
                Error = ex?.Message
            });

            rollback?.Invoke();
            throw;
        }
    } 
}