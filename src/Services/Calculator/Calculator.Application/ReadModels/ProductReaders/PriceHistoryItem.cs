namespace Calculator.Application.ReadModels.ProductReaders;

public class PriceHistoryItem
{
    public decimal Price { get; }
    public DateTime Created { get; }

    private PriceHistoryItem(decimal price)
    {
        Price = price; 
    }
    private PriceHistoryItem(decimal price, DateTime created) : this(price)
    {
        Created = created;
    }

    public static PriceHistoryItem Create(decimal price)
    {
        return new PriceHistoryItem(price);
    }

    public static PriceHistoryItem Load(decimal price, DateTime created)
    {
        return new PriceHistoryItem(price, created);
    }
}