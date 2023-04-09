using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.UpdateSettlementDay;

public record UpdateSettlementDayCommand(int NewSettlementDay, Guid Account) : ICommand<Unit>
{
    public static UpdateSettlementDayCommand Create(int newSettlementDay, Guid account)
    {
        return new UpdateSettlementDayCommand(newSettlementDay, account);
    }
}