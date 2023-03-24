namespace Calculator.Application.Features.Product.ViewModels;

public class ProductDetailsViewModel : ProductViewModel
{
    public List<ProductPriceHistoryViewModel> PriceHistory { get; init; }

    public ProductDetailsViewModel(string productSku, string companyCode, string productName, decimal pricePerUnit,
        string countedInUnit, bool isAvailable, Guid id, List<ProductPriceHistoryViewModel> productHistory) : base(
        productSku, companyCode, productName, pricePerUnit, countedInUnit, isAvailable, id)
    {
        PriceHistory = productHistory;
    }
}