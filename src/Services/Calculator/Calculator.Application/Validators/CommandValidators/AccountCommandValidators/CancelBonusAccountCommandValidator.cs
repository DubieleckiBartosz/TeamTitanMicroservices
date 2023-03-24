using Calculator.Application.Features.Account.Commands.CancelBonus;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class CancelBonusAccountCommandValidator : AbstractValidatorNotNull<CancelBonusAccountCommand>
{
    public CancelBonusAccountCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
        RuleFor(_ => _.BonusCode).StringValidator(1, 12);
    }
}