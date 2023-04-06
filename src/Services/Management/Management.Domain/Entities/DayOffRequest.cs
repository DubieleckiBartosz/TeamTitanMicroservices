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

    /// <summary>
    /// For creation
    /// </summary>
    /// <param name="createdBy"></param>
    /// <param name="daysOff"></param>
    /// <param name="reasonType"></param>
    /// <param name="description"></param>
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

    /// <summary>
    /// Load data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="consideredBy"></param>
    /// <param name="canceled"></param>
    /// <param name="daysOff"></param>
    /// <param name="currentStatus"></param>
    /// <param name="reasonType"></param>
    /// <param name="description"></param>
    private DayOffRequest(int id, string createdBy, string? consideredBy, bool canceled, RangeDaysOff daysOff,
        DayOffRequestCurrentStatus currentStatus, ReasonType reasonType, DayOffRequestDescription? description) : this(
        createdBy, daysOff, reasonType, description)
    {
        Id = id;
        ConsideredBy = consideredBy;
        Canceled = canceled;
        CurrentStatus = currentStatus;
    }

    public static DayOffRequest Create(string createdBy, RangeDaysOff daysOff, ReasonType reasonType,
        DayOffRequestDescription? description)
    {
        return new DayOffRequest(createdBy, daysOff, reasonType, description);
    }

    public static DayOffRequest Load(int id, string createdBy, string? consideredBy, bool canceled, RangeDaysOff daysOff,
        DayOffRequestCurrentStatus currentStatus, ReasonType reasonType, DayOffRequestDescription? description)
    {
        return new DayOffRequest(id, createdBy, consideredBy, canceled, daysOff,
            currentStatus, reasonType, description);
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