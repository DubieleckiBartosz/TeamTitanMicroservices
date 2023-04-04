using MediatR;

namespace Management.Application.Features.Employee.Commands.AssignAccount;

public class AssignCalculationAccountHandler : ICommandHandler<AssignCalculationAccountCommand, Unit>
{
    public Task<Unit> Handle(AssignCalculationAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}