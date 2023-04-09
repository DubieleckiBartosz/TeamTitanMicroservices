using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record PaymentMonthDayChanged(Guid AccountId, int PaymentMonthDay) : IDomainNotification; 