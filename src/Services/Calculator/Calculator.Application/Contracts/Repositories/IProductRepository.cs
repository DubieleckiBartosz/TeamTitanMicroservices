using Calculator.Application.ReadModels.ProductReaders;
using Shared.Implementations.Search;

namespace Calculator.Application.Contracts.Repositories;

public interface IProductRepository
{
    Task<bool?> ProductSkuExistsAsync(string sku);
    Task<ProductReader?> GetProductByIdAsync(Guid id);
    Task<ProductReader?> GetProductWithHistoryAsync(Guid id, string company);

    Task<ResponseSearchList<ProductReader>?> GetProductsBySearchAsync(string? productSku, decimal? pricePerUnitFrom,
        decimal? pricePerUnitTo,
        string? countedInUnit, string? productName, DateTime? fromDate, DateTime? toDate, bool? isAvailable,
        string type, string name, int pageNumber, int pageSize, string companyCode);

    Task AddAsync(ProductReader productReader);
    Task UpdatePriceAsync(ProductReader productReader);
    Task UpdateAvailabilityAsync(ProductReader productReader);
}