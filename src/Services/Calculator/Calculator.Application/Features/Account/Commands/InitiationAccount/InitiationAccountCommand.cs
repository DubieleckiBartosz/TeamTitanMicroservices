using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.InitiationAccount;

public record InitiationAccountCommand(string CompanyCode, string AccountOwnerCode, string Creator) : ICommand<Unit>
{
    public static InitiationAccountCommand Create(string companyCode, string accountOwnerCode, string creator)
    {
        return new InitiationAccountCommand(companyCode, accountOwnerCode, creator);
    }
}