using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.UpdateData;

public record UpdateAccountDataCommand(CountingType CountingType, AccountStatus Status, int WorkDayHours,
    int SettlementDayMonth, Guid AccountId, DateTime? ExpirationDate, decimal? PayoutAmount = null) : ICommand<Unit>
{
    public static UpdateAccountDataCommand Create(CountingType countingType, AccountStatus status, int workDayHours,
        int settlementDayMonth, Guid accountId, DateTime? expirationDate, decimal? payoutAmount = null)
    {
        return new UpdateAccountDataCommand(countingType, status, workDayHours,
            settlementDayMonth, accountId, expirationDate, payoutAmount);
    }
}