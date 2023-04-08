using Management.Domain.Entities;
using Management.Domain.Statuses;
using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Application.Models.DataAccessObjects;

public class DayOffRequestDao
{
    public int Id { get; init; }
    public int Version { get; init; }
    public string CreatedBy { get; init; } = default!;
    public string? ConsideredBy { get; init; }
    public bool Canceled { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
    public int CurrentStatus { get; init; }
    public int ReasonType { get; init; }
    public string? Description { get; init; }

    public DayOffRequest Map()
    {
        var daysOff = RangeDaysOff.CreateRangeDaysOff(FromDate, ToDate);
        var currentStatus = Enumeration.GetById<DayOffRequestCurrentStatus>(CurrentStatus);
        var reason = Enumeration.GetById<ReasonType>(ReasonType);
        var description = Description != null ? DayOffRequestDescription.CreateDescription(Description) : null;
        var dayOffRequest = DayOffRequest.Load(Id, Version, CreatedBy, ConsideredBy, Canceled, daysOff, currentStatus,
            reason, description);

        return dayOffRequest;
    }
}