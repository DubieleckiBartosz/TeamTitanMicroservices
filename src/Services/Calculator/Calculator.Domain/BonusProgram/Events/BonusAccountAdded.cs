using Shared.Domain.Abstractions;

namespace Calculator.Domain.BonusProgram.Events;

public record BonusAccountAdded(string Creator, string Account, Guid BonusProgramId) : IEvent
{
    public static BonusAccountAdded Create(string creator, string account, Guid bonusProgramId)
    {
        return new BonusAccountAdded(creator, account, bonusProgramId);
    }
}