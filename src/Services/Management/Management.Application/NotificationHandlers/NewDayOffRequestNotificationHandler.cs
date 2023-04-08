using Management.Application.Contracts.Services;
using Management.Application.Models.DataTransferObjects;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;

namespace Management.Application.NotificationHandlers;

public class NewDayOffRequestNotificationHandler : INotificationHandler<DomainNotification<DayOffRequestCreated>>
{
    private readonly IMessageService _messageService;

    public NewDayOffRequestNotificationHandler(IMessageService messageService)
    {
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    public async Task Handle(DomainNotification<DayOffRequestCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var message = new NewDayOffRequestMessageDto(domainEvent.EmployeeFullName, domainEvent.EmployeeCode);

        await _messageService.SendNewDayOffRequestMessage(domainEvent.Leader, message);
    }
}