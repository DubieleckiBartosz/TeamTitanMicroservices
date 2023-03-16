namespace Calculator.Domain.Account;

public class Settlement
{
    public bool IsPaid { get; private set; } 
    public DateTime From { get; }
    public DateTime To { get; }
    public decimal Value { get; }
    public string Period => $"{From.ToShortDateString()} - {To.ToShortDateString()}";

    private Settlement(bool isPaid, decimal value, DateTime from, DateTime to)
    {
        Value = value;
        IsPaid = isPaid;
        From = from;
        To = to;
    }

    public static Settlement Create(bool isPaid, decimal value, DateTime from, DateTime to)
    {
        return new Settlement(isPaid, value, from, to);
    }

    public void AsPaid()
    {
        this.IsPaid = true;
    }
}