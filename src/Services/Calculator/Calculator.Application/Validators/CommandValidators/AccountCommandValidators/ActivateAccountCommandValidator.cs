using Calculator.Application.Features.Account.Commands.ActivateAccount;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class ActivateAccountCommandValidator : AbstractValidatorNotNull<ActivateAccountCommand>
{
    public ActivateAccountCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator()); 
    }
}