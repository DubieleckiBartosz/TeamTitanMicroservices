using Newtonsoft.Json;

namespace Calculator.Application.Parameters.AccountParameters;

public class AddPieceProductParameters
{
    public Guid PieceworkProductId { get; init; }
    public decimal Quantity { get; init; }
    public decimal CurrentPrice { get; init; }
    public Guid AccountId { get; init; }
    public DateTime? Date { get; init; }

    public AddPieceProductParameters()
    {
    }

    [JsonConstructor]
    public AddPieceProductParameters(Guid pieceworkProductId, decimal quantity, decimal currentPrice, Guid accountId,
        DateTime? date)
    {
        PieceworkProductId = pieceworkProductId;
        Quantity = quantity;
        CurrentPrice = currentPrice;
        AccountId = accountId;
        Date = date;
    }
}