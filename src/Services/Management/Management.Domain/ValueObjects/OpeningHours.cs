using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class OpeningHours : ValueObject
{
    public int From { get; }
    public int To { get; }

    public OpeningHours(int from, int to)
    {
        From = from;
        To = to;
    }
    public static OpeningHours Create(int from, int to)
    {
        return new OpeningHours(from, to);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.From;
        yield return this.To;
    }
}