using Calculator.Application.Parameters.Bonus;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.AddDepartmentToBonus;

public record AddDepartmentToBonusCommand(Guid BonusProgram, string Department) : ICommand<Unit>
{
    public static AddDepartmentToBonusCommand Create(AddDepartmentToBonusParameters parameters)
    {
        return new AddDepartmentToBonusCommand(parameters.BonusProgram, parameters.Department);
    }
}