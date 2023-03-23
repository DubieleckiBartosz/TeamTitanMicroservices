namespace Calculator.Infrastructure.DataAccessObjects.AccountDataAccessObjects;

public class WorkDayDao
{
    public Guid AccountId { get; init; }
    public DateTime Date { get; init; }
    public int HoursWorked { get; init; }
    public int Overtime { get; init; }
    public bool IsDayOff { get; init; }
    public string CreatedBy { get; init; }
}