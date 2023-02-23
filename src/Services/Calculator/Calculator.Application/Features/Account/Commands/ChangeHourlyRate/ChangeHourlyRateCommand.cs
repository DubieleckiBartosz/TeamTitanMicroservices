namespace Calculator.Application.Features.Account.Commands.ChangeHourlyRate;

public record ChangeHourlyRateCommand(decimal NewHourlyRate, Guid AccountId)
{
}