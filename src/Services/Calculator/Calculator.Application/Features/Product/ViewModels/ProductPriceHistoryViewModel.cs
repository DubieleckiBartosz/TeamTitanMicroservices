namespace Calculator.Application.Features.Product.ViewModels;

public class ProductPriceHistoryViewModel
{
    public decimal Price { get; }
    public DateTime Created { get; }

    public ProductPriceHistoryViewModel(decimal price, DateTime created)
    {
        Price = price;
        Created = created;
    }
}