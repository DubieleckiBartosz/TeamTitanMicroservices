namespace Calculator.Application.ReadModels.AccountReaders;

public class ProductItemReader
{
    public Guid PieceworkProductId { get; } 
    public decimal CurrentPrice { get; }
    public decimal Quantity { get; }

    private ProductItemReader(Guid pieceworkProductId, decimal quantity, decimal currentPrice)
    {
        PieceworkProductId = pieceworkProductId;
        Quantity = quantity;
        CurrentPrice = currentPrice;
    }

    public static ProductItemReader Create(Guid pieceworkProductId, decimal quantity, decimal currentPrice) => new ProductItemReader(pieceworkProductId, quantity, currentPrice);
}