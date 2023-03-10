using MediatR;

namespace Management.Application.Features.Employee.Commands.AssignAccount;

public record AssignAccountCommand(string AccountCode, Guid AccountId) : ICommand<Unit>
{
    public static AssignAccountCommand Create(string accountCode, Guid accountId)
    {
        return new AssignAccountCommand(accountCode, accountId);
    }
}