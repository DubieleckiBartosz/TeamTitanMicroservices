namespace Calculator.Domain.Account;

public class WorkDay
{ 
    public DateTime Date { get; private init; }
    public int HoursWorked { get; private init; }
    public int Overtime { get; private init; }
    public bool IsDayOff { get; private init; }
    public string CreatedBy { get; private init; }

    public WorkDay()
    {
    }

    private WorkDay(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy)
    {
        Date = date;
        HoursWorked = hoursWorked;
        Overtime = overtime;
        IsDayOff = isDayOff;
        CreatedBy = createdBy;
    }

    public static WorkDay Create(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy)
    {
        return new WorkDay(date, hoursWorked, overtime, isDayOff, createdBy);
    }
}