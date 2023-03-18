namespace Calculator.Domain.Account;

public class Settlement
{
    public DateTime From { get; }
    public DateTime To { get; }
    public decimal Value { get; }
    public string Period => $"{From.ToShortDateString()} - {To.ToShortDateString()}";

    private Settlement(decimal value, int settlementDayMonth)
    {
        var now = DateTime.UtcNow;
        var currentMonth = now.Month;
        var currentYear = now.Year;  

        Value = value;
        To = new DateTime(currentYear, currentMonth, settlementDayMonth);
        From = To.AddMonths(-1).AddDays(-1);
    }

    public static Settlement Create(decimal value, int settlementDayMonth)
    {
        return new Settlement(value, settlementDayMonth);
    }
}