using Calculator.Application.Features.Account.Commands.ChangeOvertimeRate;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class ChangeOvertimeRateCommandValidator : AbstractValidatorNotNull<ChangeOvertimeRateCommand>
{
    public ChangeOvertimeRateCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
        RuleFor(_ => _.NewOvertimeRate).GreaterThanOrEqualTo(0); 
    }
}