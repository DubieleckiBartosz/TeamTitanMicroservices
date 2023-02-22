using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record WorkDayAdded(DateTime Date, int HoursWorked, int Overtime, bool IsDayOff, string CreatedBy,
    Guid AccountId) : IEvent
{
    public static WorkDayAdded Create(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy,
        Guid accountId)
    {
        return new WorkDayAdded(date, hoursWorked, overtime, isDayOff, createdBy, accountId);
    }
}