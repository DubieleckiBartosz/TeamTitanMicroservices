using Management.Domain.Statuses;
using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class DayOffRequest : Entity
{
    public string CreatedBy { get; }
    public string? ConsideredBy { get; private set;}
    public bool Canceled { get; private set;}
    public RangeDaysOff DaysOff { get; }
    public DayOffRequestCurrentStatus CurrentStatus { get; private set; }
    public ReasonType ReasonType { get; }
    public DayOffRequestDescription? Description { get; set; }

    private DayOffRequest(string createdBy, RangeDaysOff daysOff, ReasonType reasonType,
        DayOffRequestDescription? description)
    {
        CreatedBy = createdBy;
        DaysOff = daysOff;
        CurrentStatus = DayOffRequestCurrentStatus.Initial;
        ReasonType = reasonType;
        Description = description;
        Canceled = false;
    }

    public static DayOffRequest Create(string createdBy, RangeDaysOff daysOff, ReasonType reasonType,
        DayOffRequestDescription? description)
    {
        return new DayOffRequest(createdBy, daysOff, reasonType, description);
    }

    public void UpdateStatus(string considerBy, DayOffRequestCurrentStatus newStatus)
    { 
        ConsideredBy = considerBy;
        CurrentStatus = newStatus;
    }

    public void Cancel()
    { 
        Canceled = true;
    }
}