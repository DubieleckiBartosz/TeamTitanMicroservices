using Calculator.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Calculator.Application.Processes.ProcessingDayHours;

[EventQueue(routingKey: Keys.NewDayHoursQueueRoutingKey)]
public record DayHoursChanged(Guid AccountId, int NewWorkDayHours) : IEvent; 