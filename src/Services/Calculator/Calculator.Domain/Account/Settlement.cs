namespace Calculator.Domain.Account;

public class Settlement
{
    public DateTime From { get; private init; }
    public DateTime To { get; private init; }
    public decimal Value { get; private init; }

    public Settlement()
    {
    }

    private Settlement(decimal value, DateTime from, DateTime to)
    {
        Value = value; 
        To = to;
        From = from;
    }

    public static Settlement Create(decimal value, DateTime from, DateTime to)
    {
        return new Settlement(value, from, to);
    }
}