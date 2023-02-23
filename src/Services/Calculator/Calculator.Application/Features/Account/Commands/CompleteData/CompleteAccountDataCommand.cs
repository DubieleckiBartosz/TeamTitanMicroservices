using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Application.Features.Account.Commands.CompleteData;

public record CompleteAccountDataCommand(CountingType CountingType, AccountStatus Status, int WorkDayHours,
    decimal? OvertimeRate, decimal? HourlyRate, Guid AccountId)
{
}