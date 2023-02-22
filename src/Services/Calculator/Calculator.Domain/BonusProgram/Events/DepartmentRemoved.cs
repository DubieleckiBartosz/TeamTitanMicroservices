using Shared.Domain.Abstractions;

namespace Calculator.Domain.BonusProgram.Events;

public record DepartmentRemoved(string Department, Guid BonusProgramId) : IEvent
{
    public static DepartmentRemoved Create(string department, Guid bonusProgramId)
    {
        return new DepartmentRemoved(department, bonusProgramId);
    }
}