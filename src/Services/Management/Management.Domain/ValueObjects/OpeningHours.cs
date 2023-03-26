using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class OpeningHours : ValueObject
{
    public int From { get; }
    public int To { get; }

    private OpeningHours(int from, int to)
    {
        From = from;
        To = to;
    }
    public static OpeningHours Create(int from, int to) => new OpeningHours(from, to);

    public override string ToString()
    {
        return $"{From:00}:00 - {To:00}:00";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.From;
        yield return this.To;
    }

    protected bool Equals(OpeningHours? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        OpeningHours other = (OpeningHours)obj;
        return From == other.From && To == other.To;
    }

    public override int GetHashCode()
    {
        return From.GetHashCode() ^ To.GetHashCode();
    }
}