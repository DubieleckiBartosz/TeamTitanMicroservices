using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record BonusAdded(decimal BonusAmount, string Creator, Guid AccountId, string BonusCode) : IEvent
{
    public static BonusAdded Create(decimal bonusAmount, string creator, Guid accountId, string bonusCode)
    {
        return new BonusAdded(bonusAmount, creator, accountId, bonusCode);
    }
}