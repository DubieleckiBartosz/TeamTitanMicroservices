using Calculator.Domain.Product.Events;
using Shared.Implementations.Projection;

namespace Calculator.Application.ReadModels.ProductReaders;

public class ProductReader : IRead
{
    public Guid Id { get; }
    public string CreatedBy { get; }
    public string ProductSku { get; }
    public string CompanyCode { get; }
    public string ProductName { get; }
    public decimal PricePerUnit { get; private set; }
    public string CountedInUnit { get; private set; }
    public bool IsAvailable { get; private set; }
    public DateTime Created { get; private set; }
    public List<PriceHistoryItem> PriceHistory { get; } = new();

    /// <summary>
    /// Creating new product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="productSku"></param>
    /// <param name="companyCode"></param>
    /// <param name="productName"></param>
    /// <param name="pricePerUnit"></param>
    /// <param name="countedInUnit"></param>
    private ProductReader(Guid id, string createdBy,
        string productSku, string companyCode, string productName,
        decimal pricePerUnit, string countedInUnit)
    {
        Id = id;
        CreatedBy = createdBy;
        ProductSku = productSku;
        CompanyCode = companyCode;
        ProductName = productName;
        PricePerUnit = pricePerUnit;
        CountedInUnit = countedInUnit;
        IsAvailable = true;
        this.AddCurrentPriceToHistory();
    }

    /// <summary>
    /// Load data 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="productSku"></param>
    /// <param name="companyCode"></param>
    /// <param name="productName"></param>
    /// <param name="pricePerUnit"></param>
    /// <param name="countedInUnit"></param>
    /// <param name="isAvailable"></param>
    /// <param name="created"></param>
    /// <param name="history"></param>
    private ProductReader(Guid id, string createdBy, string productSku, string companyCode, string productName,
        decimal pricePerUnit, string countedInUnit, bool isAvailable, DateTime created, List<PriceHistoryItem>? history)
        : this(id, createdBy, productSku, companyCode, productName,
            pricePerUnit, countedInUnit)
    { 
        IsAvailable = isAvailable;
        Created = created;
        PriceHistory = history ?? new();
    }

    public static ProductReader Create(NewProductCreated @event)
    {
        return new ProductReader(@event.ProductId, @event.CreatedBy, @event.ProductSku, @event.CompanyCode,
            @event.ProductName, @event.PricePerUnit, @event.CountedInUnit);
    }

    public static ProductReader Load(Guid id, string createdBy, string productSku, string companyCode,
        string productName,
        decimal pricePerUnit, string countedInUnit, bool isAvailable, DateTime created, List<PriceHistoryItem>? history)
    {
        return new ProductReader(id, createdBy, productSku, companyCode, productName,
            pricePerUnit, countedInUnit, isAvailable, created, history);
    }

    public void PriceUpdated(ProductPriceUpdated @event)
    {
        this.AddCurrentPriceToHistory();
        PricePerUnit = @event.NewPrice;
    } 

    public void NewAvailability(AvailabilityUpdated @event)
    {
        IsAvailable = @event.IsAvailable;
    }

    private void AddCurrentPriceToHistory()
    {
        var priceHistory = PriceHistoryItem.Create(PricePerUnit);
        PriceHistory.Add(priceHistory);
    }
}