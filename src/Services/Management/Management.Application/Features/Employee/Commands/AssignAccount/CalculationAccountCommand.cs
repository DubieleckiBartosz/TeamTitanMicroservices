using MediatR;

namespace Management.Application.Features.Employee.Commands.AssignAccount;

public record CalculationAccountCommand(string OwnerVerificationCode, Guid AccountId) : ICommand<Unit>
{
    public static CalculationAccountCommand Create(string ownerVerificationCode, Guid accountId)
    {
        return new CalculationAccountCommand(ownerVerificationCode, accountId);
    }
}