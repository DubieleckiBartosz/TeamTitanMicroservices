using Management.Application.ValueTypes;
using Newtonsoft.Json;

namespace Management.Application.Parameters.EmployeeParameters;

public class DayOffRequestParameters
{ 
    public DateTime From { get; init; }
    public DateTime To { get; init; }
    public DayOffRequestReasonType ReasonType { get; init; }
    public string? Description { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public DayOffRequestParameters()
    {
    }

    [JsonConstructor]
    public DayOffRequestParameters(DateTime from, DateTime to, DayOffRequestReasonType reasonType, string? description)
    { 
        From = from;
        To = to;
        ReasonType = reasonType;
        Description = description;
    }
}