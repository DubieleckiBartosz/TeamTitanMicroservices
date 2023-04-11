using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record AccountDataUpdated(CountingType CountingType, AccountStatus Status, int WorkDayHours,
    int SettlementDayMonth, Guid AccountId, DateTime? ExpirationDate) : IEvent
{
    public static AccountDataUpdated Create(CountingType countingType, AccountStatus status, int workDayHours,
        int settlementDayMonth, Guid accountId, DateTime? expirationDate)
    {
        return new AccountDataUpdated(countingType, status, workDayHours, settlementDayMonth, accountId,
            expirationDate);
    }
}