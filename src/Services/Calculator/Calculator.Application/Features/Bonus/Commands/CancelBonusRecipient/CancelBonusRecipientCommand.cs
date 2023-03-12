using Calculator.Application.Parameters.Bonus;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.CancelBonusRecipient;

public record CancelBonusRecipientCommand(Guid BonusProgram, string RecipientCode, string BonusCode) : ICommand<Unit>
{
    public static CancelBonusRecipientCommand Create(CancelBonusRecipientParameters recipientParameters)
    {
        return new CancelBonusRecipientCommand(recipientParameters.BonusProgram, recipientParameters.RecipientCode,
            recipientParameters.BonusCode);
    }
}