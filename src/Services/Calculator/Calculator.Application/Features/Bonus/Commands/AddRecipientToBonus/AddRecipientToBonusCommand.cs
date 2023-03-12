using Calculator.Application.Parameters.Bonus;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.AddRecipientToBonus;

public record AddRecipientToBonusCommand(string Recipient, Guid BonusProgram, bool BonusGroup) : ICommand<Unit>
{
    public static AddRecipientToBonusCommand Create(AddRecipientToBonusParameters parameters)
    {
        return new AddRecipientToBonusCommand(parameters.Account, parameters.BonusProgram, parameters.BonusGroup);
    }
}