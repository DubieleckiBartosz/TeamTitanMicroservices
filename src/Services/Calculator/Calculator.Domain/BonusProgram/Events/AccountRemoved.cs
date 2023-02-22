using Shared.Domain.Abstractions;

namespace Calculator.Domain.BonusProgram.Events;

public record AccountRemoved(string Account, Guid BonusProgramId) : IEvent
{
    public static AccountRemoved Create(string account, Guid bonusProgramId)
    {
        return new AccountRemoved(account, bonusProgramId);
    }
}