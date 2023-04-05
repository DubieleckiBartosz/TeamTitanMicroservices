using Newtonsoft.Json;

namespace Management.Application.Parameters.DayOffRequestParameters;

public class CancelDayOffRequestParameters
{
    public int DayOffRequestId { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public CancelDayOffRequestParameters()
    {
    }

    [JsonConstructor]
    public CancelDayOffRequestParameters(int dayOffRequestId)
    {
        DayOffRequestId = dayOffRequestId; 
    }
}