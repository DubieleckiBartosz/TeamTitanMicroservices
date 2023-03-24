using Calculator.Application.Features.Product.ViewModels;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Product.Queries.GetProductsBySearch;

public class GetProductsBySearchQueryHandler : IQueryHandler<GetProductsBySearchQuery, ProductViewModel>
{
    public Task<ProductViewModel> Handle(GetProductsBySearchQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}