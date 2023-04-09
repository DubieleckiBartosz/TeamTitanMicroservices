using Calculator.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Calculator.Infrastructure.Processes.ProcessingPaymentDay;

[EventQueue(routingKey: Keys.NewPaymentDayQueueRoutingKey)]
public record PaymentDayChanged(Guid AccountId, int PaymentMonthDay) : IEvent; 