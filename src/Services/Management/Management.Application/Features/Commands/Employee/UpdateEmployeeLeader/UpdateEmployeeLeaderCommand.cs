using Management.Application.Parameters.EmployeeParameters;
using MediatR;

namespace Management.Application.Features.Commands.Employee.UpdateEmployeeLeader;

public record UpdateEmployeeLeaderCommand(int EmployeeId, string NewLeader) : ICommand<Unit>
{
    public static UpdateEmployeeLeaderCommand Create(UpdateEmployeeLeaderParameters parameters)
    {
        return new UpdateEmployeeLeaderCommand(parameters.EmployeeId, parameters.NewLeader);
    }
}