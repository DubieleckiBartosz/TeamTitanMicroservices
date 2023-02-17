namespace Management.Domain.ValueObjects;

public record Period
{
    public string Value { get; }

    private Period(DateTime from, DateTime to)
    {
        Value = $"{from.ToShortDateString()}-{to.ToShortDateString()}";
    }

    public static Period Create(DateTime from, DateTime to) => new Period(from, to);
}