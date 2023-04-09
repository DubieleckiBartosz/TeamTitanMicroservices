using Management.Application.NotificationHandlers.ProcessNotifications;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Management.Application.NotificationHandlers;

public class NewSettlementTypeNotificationHandler : INotificationHandler<DomainNotification<SettlementTypeChanged>>
{
    private readonly IEventBus _eventBus;

    public NewSettlementTypeNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(DomainNotification<SettlementTypeChanged> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var processEvent = new NewSettlementTypeProcess(domainEvent.AccountId, domainEvent.SettlementType); 

        await _eventBus.CommitAsync(processEvent);
    }
}