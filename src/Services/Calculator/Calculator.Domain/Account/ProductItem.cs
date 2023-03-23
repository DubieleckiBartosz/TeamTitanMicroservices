namespace Calculator.Domain.Account;

public class ProductItem
{
    public Guid PieceworkProductId { get; }

    //aggregates should not interact with each other
    //that's why we keep the price
    public decimal CurrentPrice { get; }
    public decimal Quantity { get; }
    public bool IsConsidered { get; private set; }
    public DateTime Date { get; }

    public ProductItem()
    {
    }

    private ProductItem(Guid pieceworkProductId, decimal quantity, decimal currentPrice, DateTime? date)
    {
        PieceworkProductId = pieceworkProductId;
        Quantity = quantity;
        CurrentPrice = currentPrice;
        IsConsidered = false;
        Date = date ?? DateTime.UtcNow;
    }

    public static ProductItem Create(Guid pieceworkProductId, decimal quantity, decimal currentPrice, DateTime? date)
    {
        return new ProductItem(pieceworkProductId, quantity, currentPrice, date);
    }

    public void AsConsidered()
    {
        IsConsidered = true; 
    }
}