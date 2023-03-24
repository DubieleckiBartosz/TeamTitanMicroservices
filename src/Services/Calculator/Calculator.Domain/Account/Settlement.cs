namespace Calculator.Domain.Account;

public class Settlement
{
    public DateTime From { get; }
    public DateTime To { get; }
    public decimal Value { get; }

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