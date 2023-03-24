using Calculator.Application.Features.Product.Queries.GetProductDetailsById;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.QueryValidators.ProductQueryValidators;

public class GetProductDetailsByIdQueryValidator : AbstractValidatorNotNull<GetProductDetailsByIdQuery>
{
    public GetProductDetailsByIdQueryValidator()
    {
        RuleFor(_ => _.ProductId).SetValidator(new GuidValidator()); 
    }
}