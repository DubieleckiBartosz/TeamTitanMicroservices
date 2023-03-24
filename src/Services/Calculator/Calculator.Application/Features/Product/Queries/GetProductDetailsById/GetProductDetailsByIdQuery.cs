using Calculator.Application.Features.Product.ViewModels;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Product.Queries.GetProductDetailsById;

public record GetProductDetailsByIdQuery(Guid ProductId) : IQuery<ProductDetailsViewModel>
{
    public static GetProductDetailsByIdQuery Create(Guid productId)
    {
        return new GetProductDetailsByIdQuery(productId);
    }
}