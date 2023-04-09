using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateSalary;

public class UpdateSalaryHandler : ICommandHandler<UpdateSalaryCommand, Unit>
{
    public Task<Unit> Handle(UpdateSalaryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}