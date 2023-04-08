using Calculator.Application.Features.Account.Commands.UpdateData;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Calculator.Infrastructure.Processes.ProcessingAccountDataCompletion;

public class ProcessAccountDataCompletion : IEventHandler<AccountDataCompleted>
{
    private readonly ICommandBus _commandBus;

    public ProcessAccountDataCompletion(ICommandBus commandBus)
    {
        _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
    }

    public async Task Handle(DomainNotification<AccountDataCompleted> notification, CancellationToken cancellationToken)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var data = notification.DomainEvent;

        var command = UpdateAccountDataCommand.Create((CountingType) data.CountingType, (AccountStatus) data.Status,
            data.WorkDayHours, data.SettlementDayMonth, data.AccountId, data.ExpirationDate);

        await _commandBus.Send(command, cancellationToken);
    }
}