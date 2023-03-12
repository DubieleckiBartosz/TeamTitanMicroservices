using Shared.Domain.Abstractions;

namespace Calculator.Domain.BonusProgram.Events;

public record BonusRecipientAdded(string Creator, string Recipient, Guid BonusProgramId, string BonusCode, bool GroupBonus) : IEvent
{
    public static BonusRecipientAdded Create(string creator, string recipient, Guid bonusProgramId, string bonusCode, bool groupBonus)
    {
        return new BonusRecipientAdded(creator, recipient, bonusProgramId, bonusCode, groupBonus);
    }
}