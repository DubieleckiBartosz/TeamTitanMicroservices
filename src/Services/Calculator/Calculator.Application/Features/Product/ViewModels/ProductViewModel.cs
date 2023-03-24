namespace Calculator.Application.Features.Product.ViewModels;

public class ProductViewModel
{
    public Guid Id { get; init; }
    public string ProductSku { get; init; }
    public string CompanyCode { get; init; }
    public string ProductName { get; init; }
    public decimal PricePerUnit { get; init; }
    public string CountedInUnit { get; init; }
    public bool IsAvailable { get; init; }

    public ProductViewModel(string productSku, string companyCode, string productName, decimal pricePerUnit,
        string countedInUnit, bool isAvailable, Guid id)
    {
        ProductSku = productSku;
        CompanyCode = companyCode;
        ProductName = productName;
        PricePerUnit = pricePerUnit;
        CountedInUnit = countedInUnit;
        IsAvailable = isAvailable;
        Id = id;
    }
}