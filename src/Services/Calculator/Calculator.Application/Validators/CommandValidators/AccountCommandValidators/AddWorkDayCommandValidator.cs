using Calculator.Application.Features.Account.Commands.AddWorkDay;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class AddWorkDayCommandValidator : AbstractValidatorNotNull<AddWorkDayCommand>
{
    public AddWorkDayCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
        RuleFor(_ => _.HoursWorked).InclusiveBetween(1, 12);
        RuleFor(_ => _.Overtime).InclusiveBetween(1, 12);
        RuleFor(_ => _.Date).Must(_ => _ > DateTime.UtcNow.AddMonths(-1).AddDays(-1) && _ <= DateTime.Today); 
    }
}