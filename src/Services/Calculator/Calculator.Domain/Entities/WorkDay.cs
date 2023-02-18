using Shared.Domain.Base;

namespace Calculator.Domain.Entities;

public class WorkDay : Entity // Value object?
{
    public DateTime Date { get; }
    public int HoursWorked { get; }
    public int Overtime { get; }
    public bool IsDayOff { get; }
    public int PersonWorkDay { get; }
    public string CreatedBy { get; }

    public WorkDay(DateTime date, int hoursWorked, int overtime, bool isDayOff, int personWorkDay, string createdBy)
    {
        Date = date;
        HoursWorked = hoursWorked;
        Overtime = overtime;
        IsDayOff = isDayOff;
        PersonWorkDay = personWorkDay;
        CreatedBy = createdBy;
    }
}