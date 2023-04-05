using Newtonsoft.Json;

namespace Management.Application.Parameters.DayOffRequestParameters;

public class ConsiderDayOffRequestParameters
{
    public int DayOffRequestId { get; init; } 
    public bool Positive { get; init; }
    /// <summary>
    /// For tests
    /// </summary>
    public ConsiderDayOffRequestParameters()
    {
    }

    [JsonConstructor]
    public ConsiderDayOffRequestParameters(int dayOffRequestId, bool positive)
    {
        DayOffRequestId = dayOffRequestId;
        Positive = positive;
    }
}