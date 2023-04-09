﻿using Management.Application.NotificationHandlers.ProcessNotifications;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Management.Application.NotificationHandlers;

public class NewFinancialDataNotificationHandler : INotificationHandler<DomainNotification<FinancialDataChanged>>
{
    private readonly IEventBus _eventBus;

    public NewFinancialDataNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(DomainNotification<FinancialDataChanged> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var processEvent = new NewFinancialDataProcess(domainEvent.AccountId, domainEvent.HourlyRate, domainEvent.OvertimeRate); 

        await _eventBus.CommitAsync(processEvent);
    }
}