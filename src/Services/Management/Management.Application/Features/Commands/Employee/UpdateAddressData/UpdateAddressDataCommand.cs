using Management.Application.Parameters.EmployeeParameters;
using MediatR;

namespace Management.Application.Features.Commands.Employee.UpdateAddressData;

public record UpdateAddressDataCommand(string City, string Street,
    string NumberStreet, string PostalCode, int EmployeeId) : ICommand<Unit>
{
    public static UpdateAddressDataCommand Create(UpdateAddressDataParameters parameters)
    {
        var employeeId = parameters.EmployeeId;
        var city = parameters.City;
        var street = parameters.Street;
        var numberStreet = parameters.NumberStreet;
        var postalCode = parameters.PostalCode;
        return new UpdateAddressDataCommand(city, street, numberStreet, postalCode, employeeId);
    }
}