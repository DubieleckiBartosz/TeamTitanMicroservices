using Calculator.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Calculator.Application.Processes.ProcessingFinancialData;

[EventQueue(routingKey: Keys.NewFinancialDataQueueRoutingKey)]
public record FinancialDataChanged(Guid AccountId, decimal? Salary, decimal? HourlyRate, decimal? OvertimeRate) : IEvent;