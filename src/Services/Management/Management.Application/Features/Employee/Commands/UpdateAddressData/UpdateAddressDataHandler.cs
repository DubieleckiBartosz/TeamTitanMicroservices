using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;

namespace Management.Application.Features.Employee.Commands.UpdateAddressData;

public class UpdateAddressDataHandler : ICommandHandler<UpdateAddressDataCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateAddressDataHandler(IUnitOfWork unitOfWork)
    {
        _employeeRepository = unitOfWork?.EmployeeRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Unit> Handle(UpdateAddressDataCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId);
        if (employee == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Employee"),
                Titles.MethodFailedTitle("GetEmployeeByIdAsync"));
        }

        employee.UpdateAddress(request.City, request.Street, request.NumberStreet, request.PostalCode);

        await _employeeRepository.UpdateAddressAsync(employee);

        return Unit.Value;
    }
}