namespace Calculator.Application.ReadModels.AccountReaders;

public class SettlementReader
{
    public DateTime From { get; }
    public DateTime To { get; }
    public decimal Value { get; }
    public string Period => $"{From.ToShortDateString()} - {To.ToShortDateString()}";

    private SettlementReader(decimal value, DateTime from, DateTime to)
    {
        Value = value;
        To = to;
        From = from;
    }
     
    public static SettlementReader Create(decimal value, DateTime from, DateTime to)
    {
        return new SettlementReader(value, from, to);
    } 
}