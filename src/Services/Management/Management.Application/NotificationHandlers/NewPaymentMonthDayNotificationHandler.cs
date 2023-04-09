using Management.Application.NotificationHandlers.ProcessNotifications;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Management.Application.NotificationHandlers;

public class NewPaymentMonthDayNotificationHandler : INotificationHandler<DomainNotification<PaymentMonthDayChanged>>
{
    private readonly IEventBus _eventBus;

    public NewPaymentMonthDayNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(DomainNotification<PaymentMonthDayChanged> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var processEvent = new NewPaymentMonthDayProcess(domainEvent.AccountId, domainEvent.PaymentMonthDay);

        await _eventBus.CommitAsync(processEvent);
    }
}