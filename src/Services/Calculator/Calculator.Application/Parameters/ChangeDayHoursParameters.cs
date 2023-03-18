using Newtonsoft.Json;

namespace Calculator.Application.Parameters;

public class ChangeDayHoursParameters
{
    public Guid AccountId { get; init; }
    public int NewWorkDayHours { get; init; }

    [JsonConstructor]
    public ChangeDayHoursParameters(Guid accountId, int newWorkDayHours)
    {
        AccountId = accountId;
        NewWorkDayHours = newWorkDayHours;
    }
}