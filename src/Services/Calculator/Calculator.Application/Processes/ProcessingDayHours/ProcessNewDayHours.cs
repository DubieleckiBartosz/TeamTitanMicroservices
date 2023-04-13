using Calculator.Application.Features.Account.Commands.ChangeDayHours;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Calculator.Application.Processes.ProcessingDayHours;

public class ProcessNewDayHours : IEventHandler<DayHoursChanged>
{
    private readonly ICommandBus _commandBus;

    public ProcessNewDayHours(ICommandBus commandBus)
    {
        _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
    }

    public async Task Handle(DomainNotification<DayHoursChanged> notification, CancellationToken cancellationToken)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var data = notification.DomainEvent;

        var command =
            ChangeDayHoursCommand.Create(data.NewWorkDayHours, data.AccountId);

        await _commandBus.Send(command, cancellationToken);
    }
}