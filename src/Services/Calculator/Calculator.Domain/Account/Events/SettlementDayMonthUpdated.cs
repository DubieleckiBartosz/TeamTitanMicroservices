using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record SettlementDayMonthUpdated(int SettlementDayMonth, Guid AccountId) : IEvent
{
    public static SettlementDayMonthUpdated Create(int settlementDayMonth, Guid accountId)
    {
        return new SettlementDayMonthUpdated(settlementDayMonth, accountId);
    }
}