using Calculator.Application.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.CancelBonus;

public record CancelBonusAccountCommand(Guid AccountId, string BonusCode) : ICommand<Unit>
{
    public static CancelBonusAccountCommand Create(CancelBonusAccountParameters recipientParameters)
    {
        return new CancelBonusAccountCommand(recipientParameters.AccountId, recipientParameters.BonusCode);
    }
}