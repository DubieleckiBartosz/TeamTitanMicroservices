using Calculator.Application.Features.Account.Commands.DeactivateAccount;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class DeactivateAccountCommandValidator : AbstractValidatorNotNull<DeactivateAccountCommand>
{
    public DeactivateAccountCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator()); 
    }
}