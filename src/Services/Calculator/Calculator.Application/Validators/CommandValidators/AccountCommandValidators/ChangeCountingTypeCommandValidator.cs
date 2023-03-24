using Calculator.Application.Features.Account.Commands.ChangeCountingType;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class ChangeCountingTypeCommandValidator : AbstractValidatorNotNull<ChangeCountingTypeCommand>
{
    public ChangeCountingTypeCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator()); 
    }
}