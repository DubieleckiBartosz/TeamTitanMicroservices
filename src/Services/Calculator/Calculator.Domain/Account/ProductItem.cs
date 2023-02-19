namespace Calculator.Domain.Account;

public class ProductItem
{ 
    public int PieceworkProductId { get; }
    public decimal Quantity { get; }

    private ProductItem(int pieceworkProductId, decimal quantity)
    {
        PieceworkProductId = pieceworkProductId;
        Quantity = quantity;
    }

    public static ProductItem Create(int pieceworkProductId, decimal quantity) => new ProductItem(pieceworkProductId, quantity);
}