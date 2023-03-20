namespace Calculator.Application.ReadModels.AccountReaders;

public class SettlementReader
{
    public DateTime From { get; }
    public DateTime To { get; }
    public decimal Value { get; }
    public string Period => $"{From.ToShortDateString()} - {To.ToShortDateString()}";

    private SettlementReader(decimal value, int settlementDayMonth)
    {
        var now = DateTime.UtcNow;
        var currentMonth = now.Month;
        var currentYear = now.Year;

        Value = value;
        To = new DateTime(currentYear, currentMonth, settlementDayMonth);
        From = To.AddMonths(-1).AddDays(-1);
    }

    private SettlementReader(DateTime from, DateTime to, decimal value)
    {
        From = from;
        To = to;
        Value = value;
    }

    public static SettlementReader Create(decimal value, int settlementDayMonth)
    {
        return new SettlementReader(value, settlementDayMonth);
    }

    public static SettlementReader Load(DateTime from, DateTime to, decimal value)
    {
        return new SettlementReader(from, to, value);
    }
}