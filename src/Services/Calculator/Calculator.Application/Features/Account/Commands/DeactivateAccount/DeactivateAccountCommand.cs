using Calculator.Application.Parameters.AccountParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.DeactivateAccount;

public record DeactivateAccountCommand(Guid AccountId) : ICommand<Unit>
{
    public static DeactivateAccountCommand Create(DeactivateAccountParameters parameters)
    {
        return new DeactivateAccountCommand(parameters.AccountId);
    } 
}