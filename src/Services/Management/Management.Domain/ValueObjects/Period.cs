using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class Period : ValueObject
{
    public string Value { get; }

    private Period(DateTime from, DateTime to)
    {
        Value = $"{from.ToShortDateString()}-{to.ToShortDateString()}";
    }

    public static Period Create(DateTime from, DateTime to) => new Period(from, to);
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Value;
    }
}