using MediatR;

namespace Management.Application.Features.Employee.Commands.AssignAccount;

public class CalculationAccountHandler : ICommandHandler<CalculationAccountCommand, Unit>
{
    public Task<Unit> Handle(CalculationAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}