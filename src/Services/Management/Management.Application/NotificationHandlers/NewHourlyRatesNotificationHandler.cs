using Management.Application.NotificationHandlers.ProcessNotifications;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Management.Application.NotificationHandlers;

public class NewHourlyRatesNotificationHandler : INotificationHandler<DomainNotification<HourlyRatesChanged>>
{
    private readonly IEventBus _eventBus;

    public NewHourlyRatesNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(DomainNotification<HourlyRatesChanged> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var processEvent = new NewHourlyRatesProcess(domainEvent.AccountId, domainEvent.HourlyRate, domainEvent.OvertimeRate); 

        await _eventBus.CommitAsync(processEvent);
    }
}