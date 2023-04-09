using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record HourlyRatesChanged(Guid AccountId, decimal? HourlyRate, decimal? OvertimeRate) : IDomainNotification;