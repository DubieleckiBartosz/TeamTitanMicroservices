using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.Product;

public class PieceworkProduct : Aggregate
{
    public string CreatedBy { get; }
    public string ProductCode { get; }
    public string CompanyCode { get; }
    public string ProductName { get; }
    public decimal PricePerUnit { get; private set; }
    public string CountedInUnit { get; private set; }

    private PieceworkProduct(string companyCode, decimal pricePerUnit, string countedInUnit, string productName,
        string createdBy)
    {
        CompanyCode = companyCode;
        PricePerUnit = pricePerUnit;
        CountedInUnit = countedInUnit;
        ProductName = productName;
        CreatedBy = createdBy;
        ProductCode = Guid.NewGuid().ToString(); 
    }

    public static PieceworkProduct Create(string companyCode, decimal pricePerUnit, string countedInUnit, string productName,
        string createdBy)
    {
        return new PieceworkProduct(companyCode, pricePerUnit, countedInUnit, productName, createdBy);
    }
    public void UpdatePrice()
    {
    }

    public void UpdateUnit()
    {
    }

    protected override void When(IEvent @event)
    {
        throw new NotImplementedException();
    }
}