using Shared.Domain.Base;

namespace Management.Domain.Statuses;

public class DayOffRequestCurrentStatus : Enumeration
{
    public static DayOffRequestCurrentStatus Initial = new(1, nameof(Initial));
    public static DayOffRequestCurrentStatus Accepted = new(2, nameof(Accepted));
    public static DayOffRequestCurrentStatus Rejected = new(3, nameof(Rejected));

    protected DayOffRequestCurrentStatus(int id, string name) : base(id, name)
    {
    }
}