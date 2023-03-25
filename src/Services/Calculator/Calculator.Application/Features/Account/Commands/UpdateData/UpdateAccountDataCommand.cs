using Calculator.Application.Parameters.AccountParameters;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.UpdateData;

public record UpdateAccountDataCommand(CountingType CountingType, AccountStatus Status, int WorkDayHours,
    int SettlementDayMonth, Guid AccountId, DateTime? ExpirationDate) : ICommand<Unit>
{
    public static UpdateAccountDataCommand Create(UpdateDataParameters parameters)
    {
        return new UpdateAccountDataCommand(parameters.CountingType, parameters.Status, parameters.WorkDayHours,
            parameters.SettlementDayMonth, parameters.AccountId, parameters.ExpirationDate);
    }
}