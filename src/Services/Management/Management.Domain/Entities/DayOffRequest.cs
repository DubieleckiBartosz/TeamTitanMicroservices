using Management.Domain.Statuses;
using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class DayOffRequest : Entity
{
    public string CreatedBy { get; }
    public string ConsideredBy { get; }
    public RangeDaysOff DaysOff { get; }
    public DayOffRequestCurrentStatus CurrentStatus { get; }
    public ReasonType ReasonType { get; }
    public DayOffRequestDescription? Description { get; set; }

    private DayOffRequest(string createdBy, string consideredBy, RangeDaysOff daysOff,
        DayOffRequestCurrentStatus currentStatus, ReasonType reasonType, DayOffRequestDescription? description)
    {
        CreatedBy = createdBy;
        ConsideredBy = consideredBy;
        DaysOff = daysOff;
        CurrentStatus = currentStatus;
        ReasonType = reasonType;
        Description = description;
    }

    public static DayOffRequest Create(string createdBy, string consideredBy, RangeDaysOff daysOff,
        DayOffRequestCurrentStatus currentStatus, ReasonType reasonType, DayOffRequestDescription? description)
    {
        return new DayOffRequest(createdBy, consideredBy, daysOff, currentStatus, reasonType, description);
    }


}