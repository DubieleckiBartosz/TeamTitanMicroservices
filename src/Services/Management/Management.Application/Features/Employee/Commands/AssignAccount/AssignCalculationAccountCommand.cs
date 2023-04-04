using MediatR;

namespace Management.Application.Features.Employee.Commands.AssignAccount;

public record AssignCalculationAccountCommand(string OwnerVerificationCode, Guid AccountId) : ICommand<Unit>
{
    public static AssignCalculationAccountCommand Create(string ownerVerificationCode, Guid accountId)
    {
        return new AssignCalculationAccountCommand(ownerVerificationCode, accountId);
    }
}