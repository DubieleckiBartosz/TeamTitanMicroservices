using Calculator.Application.Parameters.Bonus;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.RemoveAccountFromBonus;

public record RemoveAccountFromBonusCommand(Guid BonusProgram, string Account) : ICommand<Unit>
{
    public static RemoveAccountFromBonusCommand Create(RemoveAccountFromBonusParameters parameters)
    {
        return new RemoveAccountFromBonusCommand(parameters.BonusProgram, parameters.Account);
    }  
}