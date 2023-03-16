using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record BonusCanceled(Guid AccountId, string BonusCode) : IEvent
{
    public static BonusCanceled Create(Guid accountId, string bonusCode)
    {
        return new BonusCanceled(accountId, bonusCode);
    }
}