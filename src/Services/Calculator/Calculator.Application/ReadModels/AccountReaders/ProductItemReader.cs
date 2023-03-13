namespace Calculator.Application.ReadModels.AccountReaders;

public class ProductItemReader
{
    public Guid AccountId { get; }
    public Guid PieceworkProductId { get; }
    public decimal CurrentPrice { get; }
    public decimal Quantity { get; }

    /// <summary>
    /// For logic
    /// </summary>
    /// <param name="pieceworkProductId"></param>
    /// <param name="quantity"></param>
    /// <param name="currentPrice"></param>
    /// <param name="accountId"></param>
    private ProductItemReader(Guid pieceworkProductId, decimal quantity, decimal currentPrice, Guid accountId)
    {
        PieceworkProductId = pieceworkProductId;
        Quantity = quantity;
        CurrentPrice = currentPrice;
        AccountId = accountId;
    }

    public static ProductItemReader Create(Guid pieceworkProductId, decimal quantity, decimal currentPrice,
        Guid accountId)
    {
        return new(pieceworkProductId, quantity, currentPrice, accountId);
    }
}