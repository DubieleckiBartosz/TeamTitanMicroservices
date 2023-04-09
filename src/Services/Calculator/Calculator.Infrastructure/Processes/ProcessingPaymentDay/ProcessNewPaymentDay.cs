using Shared.Implementations.Abstractions;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Calculator.Infrastructure.Processes.ProcessingPaymentDay;

public class ProcessNewPaymentDay : IEventHandler<PaymentDayChanged>
{
    private readonly ICommandBus _commandBus;

    public ProcessNewPaymentDay(ICommandBus commandBus)
    {
        _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
    }

    public async Task Handle(DomainNotification<PaymentDayChanged> notification, CancellationToken cancellationToken)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var data = notification.DomainEvent;
    }
}