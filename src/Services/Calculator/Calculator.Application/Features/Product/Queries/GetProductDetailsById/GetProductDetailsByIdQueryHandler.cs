using Calculator.Application.Contracts;
using Calculator.Application.Features.Product.ViewModels;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Calculator.Application.Features.Product.Queries.GetProductDetailsById;

public class GetProductDetailsByIdQueryHandler : IQueryHandler<GetProductDetailsByIdQuery, ProductDetailsViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUser _currentUser;

    public GetProductDetailsByIdQueryHandler(IProductRepository productRepository, ICurrentUser currentUser)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<ProductDetailsViewModel> Handle(GetProductDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetProductWithHistoryAsync(request.ProductId, _currentUser.OrganizationCode!);

        if (result == null)
        {
            throw new NotFoundException("Product not found", "Query failed");
        }

        var historyResponse = result.PriceHistory.Select(_ => new ProductPriceHistoryViewModel(_.Price, _.Created)).ToList();

        var responseViewModel = new ProductDetailsViewModel(result.ProductSku, result.CompanyCode, result.ProductName,
            result.PricePerUnit, result.CountedInUnit, result.IsAvailable, result.Id, historyResponse);

        return responseViewModel;
    }
}