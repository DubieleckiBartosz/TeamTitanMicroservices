using Calculator.Application.Features.Account.Commands.ChangeDayHours;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class ChangeDayHoursCommandValidator : AbstractValidatorNotNull<ChangeDayHoursCommand>
{
    public ChangeDayHoursCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
        RuleFor(_ => _.NewWorkDayHours).InclusiveBetween(1, 12); 
    }
}