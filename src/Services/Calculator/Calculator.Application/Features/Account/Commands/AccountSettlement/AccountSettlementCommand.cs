using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.AccountSettlement;

public record AccountSettlementCommand(Guid AccountId) : ICommand<Unit>
{
}