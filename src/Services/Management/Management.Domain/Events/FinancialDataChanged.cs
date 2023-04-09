using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record FinancialDataChanged(Guid AccountId, decimal? HourlyRate, decimal? OvertimeRate) : IDomainNotification;