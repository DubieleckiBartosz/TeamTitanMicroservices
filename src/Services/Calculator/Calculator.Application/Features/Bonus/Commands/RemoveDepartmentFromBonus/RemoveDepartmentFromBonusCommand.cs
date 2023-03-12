using Calculator.Application.Parameters.Bonus;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.RemoveDepartmentFromBonus;

public record RemoveDepartmentFromBonusCommand(Guid BonusProgram, string DepartmentCode) : ICommand<Unit>
{
    public static RemoveDepartmentFromBonusCommand Create(RemoveDepartmentFromBonusParameters parameters)
    {
        return new RemoveDepartmentFromBonusCommand(parameters.BonusProgram, parameters.DepartmentCode);
    }
}