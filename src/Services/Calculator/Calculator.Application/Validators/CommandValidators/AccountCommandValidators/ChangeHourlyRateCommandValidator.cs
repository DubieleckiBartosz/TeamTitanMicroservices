using Calculator.Application.Features.Account.Commands.ChangeHourlyRate;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class ChangeHourlyRateCommandValidator : AbstractValidatorNotNull<ChangeHourlyRateCommand>
{
    public ChangeHourlyRateCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
        RuleFor(_ => _.NewHourlyRate).GreaterThanOrEqualTo(0); 
    }
}