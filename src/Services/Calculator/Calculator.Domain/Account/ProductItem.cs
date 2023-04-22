namespace Calculator.Domain.Account;

public class ProductItem
{
    public Guid PieceworkProductId { get; private init; }

    //aggregates should not interact with each other
    //that's why we keep the price
    public decimal CurrentPrice { get; private init; }
    public decimal Quantity { get; private init; }
    public bool IsConsidered { get; private set; }
    public DateTime Date { get; private init; }

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