using Calculator.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Calculator.Infrastructure.Processes.ProcessingFinancialData;

[EventQueue(routingKey: Keys.NewFinancialDataQueueRoutingKey)] 
public record FinancialDataChanged(Guid AccountId, decimal? HourlyRate, decimal? OvertimeRate) : IEvent;