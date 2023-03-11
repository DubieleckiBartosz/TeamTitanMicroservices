namespace Calculator.Domain.Account;

public class WorkDay
{ 
    public DateTime Date { get; }
    public int HoursWorked { get; }
    public int Overtime { get; }
    public bool IsDayOff { get; }
    public string CreatedBy { get; }

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