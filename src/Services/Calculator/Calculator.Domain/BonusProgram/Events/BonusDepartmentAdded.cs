using Shared.Domain.Abstractions;

namespace Calculator.Domain.BonusProgram.Events;

public record BonusDepartmentAdded(string Creator, string Department, Guid BonusProgramId) : IEvent
{
    public static BonusDepartmentAdded Create(string creator, string department, Guid bonusProgramId)
    {
        return new BonusDepartmentAdded(creator, department, bonusProgramId);
    }
}