using Calculator.Application.Features.Product.ViewModels;
using Calculator.Application.Parameters.AccountParameters;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Search;
using Shared.Implementations.Tools;

namespace Calculator.Application.Features.Product.Queries.GetProductsBySearch;

public record GetProductsBySearchQuery(string? ProductSku,
    decimal? PricePerUnitFrom, decimal? PricePerUnitTo,
    string? CountedInUnit, string? ProductName, DateTime? FromDate, DateTime? ToDate,
    bool IsAvailable, SortModel Sort, int PageNumber, int PageSize) : IQuery<ProductViewModel>
{
    public static GetProductsBySearchQuery Create(GetProductsBySearchParameters parameters)
    {
        var productSku = parameters.ProductSku;
        var pricePerUnitFrom = parameters.PricePerUnitFrom;
        var pricePerUnitTo = parameters.PricePerUnitTo;
        var countedInUnit = parameters.CountedInUnit;
        var productName = parameters.ProductName;
        var fromDate = parameters.FromDate;
        var toDate = parameters.ToDate;
        var isAvailable = parameters.IsAvailable;
        var sort = parameters.CheckOrAssignSortModel();
        var pageNumber = parameters.PageNumber;
        var pageSize = parameters.PageSize;

        return new GetProductsBySearchQuery(productSku, pricePerUnitFrom, pricePerUnitTo, countedInUnit, productName,
            fromDate, toDate, isAvailable, sort, pageNumber, pageSize);
    }
}