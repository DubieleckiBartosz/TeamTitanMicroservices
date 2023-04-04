using Management.Application.Parameters.EmployeeParameters;

namespace Management.Application.Features.Commands.Employee.CreateEmployee;

public record CreateEmployeeCommand(int DepartmentId, string Name, string Surname,
    DateTime Birthday, string? PersonIdentifier, string City, string Street, string NumberStreet, string PostalCode,
    string PhoneNumber, string Email) : ICommand<string>
{
    public static CreateEmployeeCommand Create(CreateEmployeeParameters parameters)
    {
        return new CreateEmployeeCommand(parameters.DepartmentId,
            parameters.Name, parameters.Surname,
            parameters.Birthday, parameters.PersonIdentifier,
            parameters.City, parameters.Street,
            parameters.NumberStreet, parameters.PostalCode,
            parameters.PhoneNumber, parameters.Email);
    }
}