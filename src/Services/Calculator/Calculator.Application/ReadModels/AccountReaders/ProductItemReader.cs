namespace Calculator.Application.ReadModels.AccountReaders;

public class ProductItemReader
{
    public Guid Id { get; }
    public Guid AccountId { get; }
    public Guid PieceworkProductId { get; }
    public decimal CurrentPrice { get; }
    public decimal Quantity { get; }
    public bool IsConsidered { get; private set; }
    public DateTime Date { get; }

    /// <summary>
    /// For logic
    /// </summary>
    /// <param name="pieceworkProductId"></param>
    /// <param name="quantity"></param>
    /// <param name="currentPrice"></param>
    /// <param name="accountId"></param>
    /// <param name="date"></param>
    private ProductItemReader(Guid pieceworkProductId, decimal quantity, decimal currentPrice, Guid accountId, DateTime date)
    {
        PieceworkProductId = pieceworkProductId;
        Quantity = quantity;
        CurrentPrice = currentPrice;
        AccountId = accountId;
        Date = date;
    }

    /// <summary>
    ///     For load
    /// </summary>
    /// <param name="id"></param>
    /// <param name="accountId"></param>
    /// <param name="pieceworkProductId"></param>
    /// <param name="currentPrice"></param>
    /// <param name="quantity"></param>
    /// <param name="isConsidered"></param>
    /// <param name="date"></param>
    private ProductItemReader(Guid id, Guid accountId, Guid pieceworkProductId, decimal currentPrice, decimal quantity,
        bool isConsidered, DateTime date) : this(pieceworkProductId, quantity, currentPrice, accountId, date)
    {
        IsConsidered = isConsidered;
        Id = id;
    }

    public static ProductItemReader Create(Guid pieceworkProductId, decimal quantity, decimal currentPrice,
        Guid accountId, DateTime date)
    {
        return new ProductItemReader(pieceworkProductId, quantity, currentPrice, accountId, date);
    }

    public static ProductItemReader Load(Guid id, Guid pieceworkProductId, decimal quantity, decimal currentPrice,
        Guid accountId, bool isConsidered, DateTime date)
    {
        return new ProductItemReader(id, accountId, pieceworkProductId, currentPrice, quantity, isConsidered, date);
    }

    public void AsConsidered()
    {
        IsConsidered = true;
    }
}