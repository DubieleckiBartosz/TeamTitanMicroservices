using Shared.Domain.Base;

namespace Calculator.Domain.Product;

public class PieceworkProduct : Entity
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
        Watcher = Watcher.Create();
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
}