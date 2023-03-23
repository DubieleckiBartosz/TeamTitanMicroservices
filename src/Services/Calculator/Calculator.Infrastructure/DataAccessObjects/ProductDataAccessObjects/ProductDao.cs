using Calculator.Application.ReadModels.ProductReaders;

namespace Calculator.Infrastructure.DataAccessObjects.ProductDataAccessObjects;

public class ProductDao
{
    public Guid Id { get; init; }
    public string CreatedBy { get; init; }
    public string ProductSku { get; init; }
    public string CompanyCode { get; init; }
    public string ProductName { get; init; }
    public decimal PricePerUnit { get; init; }
    public string CountedInUnit { get; init; }
    public bool IsAvailable { get; init; }
    public DateTime Created { get; init; }
    public List<PriceHistoryDao> PriceHistory { get; set; } = new();

    public ProductReader Map()
    {
        var priceHistory = PriceHistory?.Select(_ => PriceHistoryItem.Load(_.Price, _.Created)).ToList();
        return ProductReader.Load(Id, CreatedBy, ProductSku, CompanyCode, ProductName, PricePerUnit, CountedInUnit,
            IsAvailable, Created, priceHistory);
    }
}