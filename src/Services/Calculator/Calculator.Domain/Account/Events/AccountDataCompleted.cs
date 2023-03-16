using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record AccountDataCompleted(CountingType CountingType, AccountStatus Status, int WorkDayHours,
    decimal? OvertimeRate, decimal? HourlyRate, Guid AccountId, DateTime? ExpirationDate) : IEvent
{
    public static AccountDataCompleted Create(CountingType countingType, AccountStatus status, int workDayHours,
        decimal? overtimeRate, decimal? hourlyRate, Guid accountId, DateTime? expirationDate)
    {
        return new AccountDataCompleted(countingType, status, workDayHours, overtimeRate, hourlyRate, accountId,
            expirationDate);
    }
}