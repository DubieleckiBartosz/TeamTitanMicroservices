using Management.Application.NotificationHandlers.ProcessNotifications;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Management.Application.NotificationHandlers;

public class NewContractNotificationHandler : INotificationHandler<DomainNotification<ContractCreated>>
{
    private readonly IEventBus _eventBus;

    public NewContractNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(DomainNotification<ContractCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var processEvent = new ContractProcess(domainEvent.CountingType, domainEvent.WorkDayHours,
            domainEvent.SettlementDayMonth, domainEvent.AccountId, domainEvent.ExpirationDate);

        await _eventBus.CommitAsync(processEvent);
    }
}