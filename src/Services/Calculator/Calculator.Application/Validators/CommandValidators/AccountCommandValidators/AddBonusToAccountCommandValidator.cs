using Calculator.Application.Features.Account.Commands.AddBonus;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class AddBonusToAccountCommandValidator : AbstractValidatorNotNull<AddBonusToAccountCommand>
{
    public AddBonusToAccountCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
        RuleFor(_ => _.Amount).GreaterThan(0);
    }
}