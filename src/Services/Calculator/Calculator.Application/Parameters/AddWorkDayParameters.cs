using Newtonsoft.Json;

namespace Calculator.Application.Parameters;

public class AddWorkDayParameters
{
    public DateTime Date { get; init; }
    public int HoursWorked { get; init; }
    public int Overtime { get; init; }
    public bool IsDayOff { get; init; }
    public Guid AccountId { get; init; }

    [JsonConstructor]
    public AddWorkDayParameters(DateTime date, int hoursWorked, int overtime, bool isDayOff, Guid accountId)
    {
        Date = date;
        HoursWorked = hoursWorked;
        Overtime = overtime;
        IsDayOff = isDayOff;
        AccountId = accountId;
    }
}