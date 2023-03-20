namespace Calculator.Infrastructure.DataAccessObjects;

public class SettlementDao
{
    public DateTime From { get; init; }
    public DateTime To { get; init; }
    public decimal Value { get; init; }
}