namespace Calculator.Domain.Account;

public class ProductItem
{ 
    public Guid PieceworkProductId { get; }
    //aggregates should not interact with each other
    //that's why we keep the price
    public decimal CurrentPrice { get; }
    public decimal Quantity { get; }

    private ProductItem(Guid pieceworkProductId, decimal quantity, decimal currentPrice)
    {
        PieceworkProductId = pieceworkProductId;
        Quantity = quantity;
        CurrentPrice = currentPrice;
    }

    public static ProductItem Create(Guid pieceworkProductId, decimal quantity, decimal currentPrice) => new ProductItem(pieceworkProductId, quantity, currentPrice);
}