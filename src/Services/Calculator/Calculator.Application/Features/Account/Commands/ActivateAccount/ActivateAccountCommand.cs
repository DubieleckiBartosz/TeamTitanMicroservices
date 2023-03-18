using Calculator.Application.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ActivateAccount;

public record ActivateAccountCommand(Guid AccountId) : ICommand<Unit>
{
    public static ActivateAccountCommand Create(ActivateAccountParameters parameters)
    {
        return new ActivateAccountCommand(parameters.AccountId);
    }
}