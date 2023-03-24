using Calculator.Application.ReadModels.ProductReaders;

namespace Calculator.Application.Contracts;

public interface IProductRepository
{
    Task<ProductReader?> GetProductByIdAsync(Guid id);
    Task<ProductReader?> GetProductWithHistoryAsync(Guid id, string company);
    Task<List<ProductReader>?> GetProductsBySearchAsync();
    Task AddAsync(ProductReader productReader);
    Task UpdatePriceAsync(ProductReader productReader);
    Task UpdateAvailabilityAsync(ProductReader productReader);
}