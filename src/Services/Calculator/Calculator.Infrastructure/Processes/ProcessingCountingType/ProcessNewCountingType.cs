using Calculator.Application.Features.Account.Commands.ChangeCountingType;
using Calculator.Domain.Types;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Calculator.Infrastructure.Processes.ProcessingCountingType;

public class ProcessNewCountingType : IEventHandler<CountingTypeChanged>
{
    private readonly ICommandBus _commandBus;

    public ProcessNewCountingType(ICommandBus commandBus)
    {
        _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
    }

    public async Task Handle(DomainNotification<CountingTypeChanged> notification, CancellationToken cancellationToken)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var data = notification.DomainEvent;

        var newCountingType = (CountingType)data.SettlementType;
        var command = ChangeCountingTypeCommand.Create(newCountingType, data.AccountId);

        await _commandBus.Send(command, cancellationToken);
    }
}