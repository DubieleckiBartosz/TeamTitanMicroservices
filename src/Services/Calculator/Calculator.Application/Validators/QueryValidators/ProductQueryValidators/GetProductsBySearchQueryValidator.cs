using Calculator.Application.Features.Product.Queries.GetProductsBySearch;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.QueryValidators.ProductQueryValidators;

public class GetProductsBySearchQueryValidator : AbstractValidatorNotNull<GetProductsBySearchQuery>
{
    private readonly string[] _availableNames = new[]
    {
        "Id", "Created", "Price", "ProductName", "Availability"
    };

    public GetProductsBySearchQueryValidator()
    {
        When(_ => _.FromDate != null && _.ToDate != null, () =>
        {
            RuleFor(r => r.FromDate).LessThanOrEqualTo(x => x.ToDate);
        });
        
        When(_ => _.ToDate != null && _.FromDate != null, () =>
        {
            RuleFor(r => r.ToDate).GreaterThanOrEqualTo(x => x.FromDate);
        });

        When(_ => _.ProductName != null, () =>
        {
            RuleFor(_ => _.ProductName!).StringValidator();
        });

        this.When(_ => _.Sort?.Name != null,
            () => this.RuleFor(r => r.Sort).SetValidator(new SortValidator(this._availableNames)!));
    }
}