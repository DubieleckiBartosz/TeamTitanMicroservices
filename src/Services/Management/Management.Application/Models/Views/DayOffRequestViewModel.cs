using Management.Application.ValueTypes;

namespace Management.Application.Models.Views;

public class DayOffRequestViewModel
{
    public int Id { get; init; }
    public string CreatedBy { get; init; } = default!;
    public string? ConsideredBy { get; init; }
    public bool Canceled { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
    public int CurrentStatus { get; init; }
    public ReasonType ReasonType { get; init; }
    public string? Description { get; init; }
}