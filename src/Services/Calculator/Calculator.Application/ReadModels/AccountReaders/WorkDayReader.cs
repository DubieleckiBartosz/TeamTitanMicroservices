namespace Calculator.Application.ReadModels.AccountReaders;

public class WorkDayReader
{
    public Guid AccountId { get; }
    public DateTime Date { get; }
    public int HoursWorked { get; }
    public int Overtime { get; }
    public bool IsDayOff { get; }
    public string CreatedBy { get; }

    private WorkDayReader(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy, Guid accountId)
    {
        AccountId = accountId;
        Date = date;
        HoursWorked = hoursWorked;
        Overtime = overtime;
        IsDayOff = isDayOff;
        CreatedBy = createdBy;
    }

    public static WorkDayReader Create(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy,
        Guid accountId)
    {
        return new WorkDayReader(date, hoursWorked, overtime, isDayOff, createdBy, accountId);
    }
}