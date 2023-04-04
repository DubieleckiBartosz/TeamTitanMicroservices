using Management.Application.Parameters.EmployeeParameters;
using MediatR;

namespace Management.Application.Features.Employee.Commands.UpdateContactData;

public record UpdateContactDataCommand(string? PhoneNumber, string? Email, int EmployeeId) : ICommand<Unit>
{
    public static UpdateContactDataCommand Create(UpdateContactDataParameters parameters)
    {
        return new UpdateContactDataCommand(parameters.PhoneNumber, parameters.Email, parameters.EmployeeId);
    }
}