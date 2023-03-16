using Calculator.Application.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.AddBonus;

public record AddBonusToAccountCommand(Guid AccountId, decimal Amount) : ICommand<Unit>
{
    public static AddBonusToAccountCommand Create(AddBonusToAccountParameters parameters)
    {
        return new AddBonusToAccountCommand(parameters.AccountId, parameters.Amount);
    }
}