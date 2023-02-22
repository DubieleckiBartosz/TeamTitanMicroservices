using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record DayHoursChanged(int NewWorkDayHours, Guid AccountId) : IEvent
{
    public static DayHoursChanged Create(int newWorkDayHours, Guid accountId)
    {
        return new DayHoursChanged(newWorkDayHours, accountId);
    }
}