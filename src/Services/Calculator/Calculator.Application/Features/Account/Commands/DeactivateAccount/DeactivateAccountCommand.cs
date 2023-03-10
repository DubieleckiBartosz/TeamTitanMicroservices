using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.DeactivateAccount;

public record DeactivateAccountCommand(Guid AccountId) : ICommand<Unit>
{
}