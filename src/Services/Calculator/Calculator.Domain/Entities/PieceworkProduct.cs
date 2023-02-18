using Shared.Domain.Base;

namespace Calculator.Domain.Entities;

public class PieceworkProduct : Entity
{
    public string CompanyCode { get; }
    public decimal PricePerUnit { get; }
    public string CountedInUnit { get; }
    public string ProductName { get; }

    public PieceworkProduct(string companyCode, decimal pricePerUnit, string countedInUnit, string productName)
    {
        CompanyCode = companyCode;
        PricePerUnit = pricePerUnit;
        CountedInUnit = countedInUnit;
        ProductName = productName;
    }
}