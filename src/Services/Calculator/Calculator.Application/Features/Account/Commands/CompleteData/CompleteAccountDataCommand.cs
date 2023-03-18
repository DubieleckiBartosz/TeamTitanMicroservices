using Calculator.Application.Parameters;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.CompleteData;

public record CompleteAccountDataCommand(CountingType CountingType, AccountStatus Status, int WorkDayHours,
    int SettlementDayMonth, Guid AccountId, DateTime? ExpirationDate) : ICommand<Unit>
{
    public static CompleteAccountDataCommand Create(CompleteDataParameters parameters)
    {
        return new CompleteAccountDataCommand(parameters.CountingType, parameters.Status, parameters.WorkDayHours,
            parameters.SettlementDayMonth, parameters.AccountId, parameters.ExpirationDate);
    }
}