using Calculator.Application.Parameters.Bonus;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.AddAccountToBonus;

public record AddAccountToBonusCommand(string Account, Guid BonusProgram) : ICommand<Unit>
{
    public static AddAccountToBonusCommand Create(AddAccountToBonusParameters parameters)
    {
        return new AddAccountToBonusCommand(parameters.Account, parameters.BonusProgram);
    }
}