using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ActivateAccount;

public record ActivateAccountCommand(Guid AccountId) : ICommand<Unit>
{
}