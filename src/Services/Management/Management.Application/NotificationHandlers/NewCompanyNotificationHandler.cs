using Management.Application.Contracts.Services;
using Management.Application.Models.DataTransferObjects;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;
using Shared.Implementations.Services;

namespace Management.Application.NotificationHandlers;

public class NewCompanyNotificationHandler : INotificationHandler<DomainNotification<CompanyDeclared>>
{
    private readonly IMessageService _messageService;
    private readonly ICurrentUser _currentUser;

    public NewCompanyNotificationHandler(IMessageService messageService, ICurrentUser currentUser)
    {
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task Handle(DomainNotification<CompanyDeclared> notification, CancellationToken cancellationToken)
    { 
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var recipient = _currentUser.Email;
        var message = new InitCompanyMessageDto(domainEvent.CompanyCode, domainEvent.OwnerCode, recipient: recipient);
        await _messageService.SendInitCompanyMessage(recipient, message);
    }
}