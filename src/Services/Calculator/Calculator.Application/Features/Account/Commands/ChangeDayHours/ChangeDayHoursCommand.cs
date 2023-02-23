namespace Calculator.Application.Features.Account.Commands.ChangeDayHours;

public record ChangeDayHoursCommand(int NewWorkDayHours, Guid AccountId)
{
}