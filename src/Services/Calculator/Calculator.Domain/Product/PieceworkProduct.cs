using Calculator.Domain.Product.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace Calculator.Domain.Product;

public class PieceworkProduct : Aggregate
{
    public string CreatedBy { get; private set; }
    public string ProductSku { get; private set; }
    public string CompanyCode { get; private set; }
    public string ProductName { get; private set; }
    public decimal PricePerUnit { get; private set; } 
    public string CountedInUnit { get; private set; }
    public bool IsAvailable { get; private set; }
    public List<decimal> PriceHistory { get; private set; } = new();

    //Constructor for serialization
    public PieceworkProduct()
    {
    }

    /// <summary>
    /// For logic
    /// </summary>
    /// <param name="companyCode"></param>
    /// <param name="pricePerUnit"></param>
    /// <param name="countedInUnit"></param>
    /// <param name="productName"></param>
    /// <param name="createdBy"></param>
    /// <param name="productSku"></param>
    private PieceworkProduct(string companyCode, decimal pricePerUnit, string countedInUnit, string productName,
        string createdBy, string productSku)
    { 
        var @event = NewProductCreated.Create(companyCode, pricePerUnit, countedInUnit, productName, createdBy,
            productSku, Guid.NewGuid());

        this.Apply(@event);
        this.Enqueue(@event);
    }

    public static PieceworkProduct Create(string companyCode, decimal pricePerUnit, string countedInUnit, string productName,
        string createdBy, string productSku)
    {
        return new PieceworkProduct(companyCode, pricePerUnit, countedInUnit, productName, createdBy, productSku);
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (!IsAvailable)
        {
            throw new BusinessException("Incorrect availability",
                "The product must be available during the price change.");
        }

        var @event = ProductPriceUpdated.Create(newPrice, this.Id);

        this.Apply(@event);
        this.Enqueue(@event);
    }
    
    public void UpdateAvailability()
    {
        var @event = AvailabilityUpdated.Create(!IsAvailable, this.Id);

        this.Apply(@event);
        this.Enqueue(@event);
    }

    public override ISnapshot? CreateSnapshot()
    {
        return null;
    }

    public override Aggregate? FromSnapshot(ISnapshot snapshot)
    {
        return null;
    }

    protected override void When(IEvent @event)
    {
        switch (@event)
        {
            case NewProductCreated e:
                this.ProductCreated(e);
                break;
            case ProductPriceUpdated e:
                this.PriceUpdated(e);
                break; 
            case AvailabilityUpdated e:
                this.NewAvailability(e);
                break; 
            default:
                break;
        }
    }

    public void PriceUpdated(ProductPriceUpdated @event)
    {
        PriceHistory.Add(PricePerUnit);
        PricePerUnit = @event.NewPrice;
    }

    public void ProductCreated(NewProductCreated @event)
    {
        Id = @event.ProductId;
        CompanyCode = @event.CompanyCode;
        PricePerUnit = @event.PricePerUnit;
        CountedInUnit = @event.CountedInUnit;
        ProductName = @event.ProductName;
        CreatedBy = @event.CreatedBy;
        ProductSku = @event.ProductSku;
        IsAvailable = true;
        PriceHistory.Add(PricePerUnit);
    }

    public void NewAvailability(AvailabilityUpdated @event)
    { 
        IsAvailable = @event.IsAvailable;
    }
}