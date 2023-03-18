using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record AccountDataCompleted(CountingType CountingType, AccountStatus Status, int WorkDayHours,
    int SettlementDayMonth, Guid AccountId, DateTime? ExpirationDate) : IEvent
{
    public static AccountDataCompleted Create(CountingType countingType, AccountStatus status, int workDayHours,
        int settlementDayMonth, Guid accountId, DateTime? expirationDate)
    {
        return new AccountDataCompleted(countingType, status, workDayHours, settlementDayMonth, accountId,
            expirationDate);
    }
}