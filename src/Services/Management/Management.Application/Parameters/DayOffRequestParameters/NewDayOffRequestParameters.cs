using Management.Application.ValueTypes;
using Newtonsoft.Json;

namespace Management.Application.Parameters.DayOffRequestParameters;

public class NewDayOffRequestParameters
{
    public DateTime From { get; init; }
    public DateTime To { get; init; }
    public DayOffRequestReasonType ReasonType { get; init; }
    public string? Description { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public NewDayOffRequestParameters()
    {
    }

    [JsonConstructor]
    public NewDayOffRequestParameters(DateTime from, DateTime to, DayOffRequestReasonType reasonType, string? description)
    {
        From = from;
        To = to;
        ReasonType = reasonType;
        Description = description;
    }
}