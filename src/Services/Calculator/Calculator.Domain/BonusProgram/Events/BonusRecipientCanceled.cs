using Shared.Domain.Abstractions;

namespace Calculator.Domain.BonusProgram.Events;

public record BonusRecipientCanceled(string Recipient, Guid BonusProgramId, string BonusCode) : IEvent
{
    public static BonusRecipientCanceled Create(string recipient, Guid bonusProgramId, string bonusCode)
    {
        return new BonusRecipientCanceled(recipient, bonusProgramId, bonusCode);
    }
}