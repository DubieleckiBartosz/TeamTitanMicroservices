using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class RangeDaysOff : ValueObject
{
    public DateTime FromDate { get; }
    public DateTime ToDate { get; }

    private RangeDaysOff(DateTime fromDate, DateTime toDate)
    {
        FromDate = fromDate;
        ToDate = toDate;
    }

    public static RangeDaysOff CreateRangeDaysOff(DateTime fromDate, DateTime toDate)
    {
        return new RangeDaysOff(fromDate, toDate);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.FromDate;
        yield return this.ToDate;
    }
}