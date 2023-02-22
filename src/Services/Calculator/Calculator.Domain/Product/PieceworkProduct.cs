using Calculator.Domain.Product.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.Product;

public class PieceworkProduct : Aggregate
{
    public string CreatedBy { get; private set; }
    public string ProductCode { get; private set; }
    public string CompanyCode { get; private set; }
    public string ProductName { get; private set; }
    public decimal PricePerUnit { get; private set; }
    public string CountedInUnit { get; private set; }

    private PieceworkProduct(string companyCode, decimal pricePerUnit, string countedInUnit, string productName,
        string createdBy)
    {
        var productCode = Guid.NewGuid().ToString();
        var @event = NewProductCreated.Create(companyCode, pricePerUnit, countedInUnit, productName, createdBy,
            productCode,
            Guid.NewGuid());

        this.Apply(@event);
        this.Enqueue(@event);
    }

    public static PieceworkProduct Create(string companyCode, decimal pricePerUnit, string countedInUnit, string productName,
        string createdBy)
    {
        return new PieceworkProduct(companyCode, pricePerUnit, countedInUnit, productName, createdBy);
    }
    public void UpdatePrice(decimal newPrice)
    {
        var @event = ProductPriceUpdated.Create(newPrice, this.Id);

        this.Apply(@event);
        this.Enqueue(@event);
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
            default:
                break;
        }
    }

    public void PriceUpdated(ProductPriceUpdated @event)
    {
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
        ProductCode = @event.ProductCode;
    }

}