namespace Calculator.Application.Features.Account.Commands.ChangeOvertimeRate;

public record ChangeOvertimeRateCommand(decimal NewOvertimeRate, Guid AccountId)
{
}