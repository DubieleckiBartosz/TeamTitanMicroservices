using Newtonsoft.Json;
using Shared.Implementations.Search;
using Shared.Implementations.Search.SearchParameters;

namespace Calculator.Application.Parameters.ProductParameters;

public class GetProductsBySearchParameters : BaseSearchQueryParameters, IFilterModel
{
    public string? ProductSku { get; init; }
    public decimal? PricePerUnitFrom { get; init; }
    public decimal? PricePerUnitTo { get; init; }
    public string? CountedInUnit { get; init; }
    public string? ProductName { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public bool IsAvailable { get; init; }
    public SortModelParameters Sort { get; set; }

    [JsonConstructor]
    public GetProductsBySearchParameters(string? productSku, decimal? pricePerUnitFrom, decimal? pricePerUnitTo,
        string? countedInUnit, string? productName, DateTime? fromDate, DateTime? toDate, bool isAvailable,
        SortModelParameters sort, int pageNumber, int pageSize)
    {
        ProductSku = productSku;
        PricePerUnitFrom = pricePerUnitFrom;
        PricePerUnitTo = pricePerUnitTo;
        CountedInUnit = countedInUnit;
        ProductName = productName;
        FromDate = fromDate;
        ToDate = toDate;
        IsAvailable = isAvailable;
        Sort = sort;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}