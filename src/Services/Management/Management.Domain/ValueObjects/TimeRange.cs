namespace Management.Domain.ValueObjects;

public record TimeRange
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
}