using Calculator.Application.Constants;
using Calculator.Application.Contracts.Repositories;
using Calculator.Application.Features.Product.ViewModels;
using Calculator.Application.ReadModels.ProductReaders;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Mappings;
using Shared.Implementations.Services;

namespace Calculator.Application.Features.Product.Queries.GetProductsBySearch;

public class GetProductsBySearchQueryHandler : IQueryHandler<GetProductsBySearchQuery, ProductSearchViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUser _currentUser;

    public GetProductsBySearchQueryHandler(IProductRepository productRepository,ICurrentUser currentUser)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<ProductSearchViewModel> Handle(GetProductsBySearchQuery request, CancellationToken cancellationToken)
    {
        var productSku = request.ProductSku;
        var pricePerUnitFrom = request.PricePerUnitFrom;
        var pricePerUnitTo = request.PricePerUnitTo;
        var countedInUnit = request.CountedInUnit;
        var productName = request.ProductName;
        var fromDate = request.FromDate;
        var toDate = request.ToDate;
        var isAvailable = request.IsAvailable;
        var type = request.Sort.Type;
        var name = request.Sort.Name;
        var pageNumber = request.PageNumber;
        var pageSize = request.PageSize;

        var result = await _productRepository.GetProductsBySearchAsync(
            productSku, pricePerUnitFrom, pricePerUnitTo,
            countedInUnit, productName, fromDate, toDate, isAvailable,
            type, name, pageNumber, pageSize, _currentUser.OrganizationCode!);

        if (result == null)
        {
            throw new NotFoundException(Messages.ProductsNotFoundMessage, Titles.DataNotFoundTitle);
        }

        var data = result.Data;
        var count = result.TotalCount;
        var products = Mapping.MapList<ProductReader, ProductViewModel>(data);

        return new ProductSearchViewModel(count, products);
    }
}