using Calculator.Application.Features.Account.Queries.GetAccountsBySearch;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.QueryValidators.AccountQueryValidators;

public class GetAccountsBySearchQueryValidator : AbstractValidatorNotNull<GetAccountsBySearchQuery>
{
    private readonly string[] _availableNames =
    {
        "Id", "HourlyRate", "ExpirationDate", "Balance", "SettlementDayMonth", "Created"
    };

    public GetAccountsBySearchQueryValidator()
    {
        When(_ => _.ExpirationDateFrom != null && _.ExpirationDateTo != null,
            () =>
            {
                RuleFor(r => r.ExpirationDateFrom)
                    .LessThanOrEqualTo(x => x.ExpirationDateTo);
            });

        When(_ => _.ExpirationDateTo != null && _.ExpirationDateFrom != null,
            () =>
            {
                RuleFor(r => r.ExpirationDateTo)
                    .GreaterThanOrEqualTo(x => x.ExpirationDateFrom);
            });

        When(_ => _.HourlyRateFrom != null && _.HourlyRateTo != null,
            () =>
            {
                RuleFor(r => r.HourlyRateFrom)
                    .LessThanOrEqualTo(x => x.HourlyRateTo)
                    .GreaterThan(0);
            });

        When(_ => _.HourlyRateTo != null && _.HourlyRateFrom != null,
            () =>
            {
                RuleFor(r => r.HourlyRateTo)
                    .GreaterThanOrEqualTo(x => x.HourlyRateFrom)
                    .GreaterThan(0);
            });

        When(_ => _.BalanceFrom != null && _.BalanceTo != null,
            () =>
            {
                RuleFor(r => r.BalanceFrom)
                    .LessThanOrEqualTo(x => x.BalanceTo)
                    .GreaterThan(0);
            });

        When(_ => _.BalanceTo != null && _.BalanceFrom != null,
            () =>
            {
                RuleFor(r => r.BalanceTo)
                    .GreaterThanOrEqualTo(x => x.BalanceFrom)
                    .GreaterThan(0);
            });

        When(_ => _.SettlementDayMonth != null, () => RuleFor(_ => _.SettlementDayMonth!).GreaterThan(0)); 
        When(_ => _.ActivatedBy != null, () => RuleFor(_ => _.ActivatedBy!).StringValidator());
        When(_ => _.DeactivatedBy != null, () => RuleFor(_ => _.DeactivatedBy!).StringValidator());
        When(_ => _.AccountId.HasValue, () => RuleFor(_ => _.AccountId!.Value).SetValidator(new GuidValidator()));
        When(_ => _.Sort?.Name != null,
            () => RuleFor(r => r.Sort).SetValidator(new SortValidator(_availableNames)!));
    }
}