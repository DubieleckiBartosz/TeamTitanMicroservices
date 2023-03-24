using Calculator.Application.Features.Account.Commands.CompleteData;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class CompleteAccountDataCommandValidator : AbstractValidatorNotNull<CompleteAccountDataCommand>
{
    public CompleteAccountDataCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
        RuleFor(_ => _.WorkDayHours).InclusiveBetween(1, 12);
        RuleFor(_ => _.SettlementDayMonth).InclusiveBetween(1, 28);
        When(_ => _.ExpirationDate != null,
            () => RuleFor(_ => _.ExpirationDate!).GreaterThan(DateTime.UtcNow.AddDays(1)));
    }
}