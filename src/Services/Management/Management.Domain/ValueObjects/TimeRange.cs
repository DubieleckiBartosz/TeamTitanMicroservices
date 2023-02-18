using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class TimeRange : ValueObject
{
    public DateTime StartContract { get; }
    public DateTime? EndContract { get; }

    private TimeRange(DateTime startContract, DateTime? endContract)
    {
        StartContract = startContract;
        EndContract = endContract;
    }

    public static TimeRange Create(DateTime startContract, DateTime? endContract)
    {
        return new TimeRange(startContract, endContract);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.StartContract;
        yield return this.EndContract;
    }
}