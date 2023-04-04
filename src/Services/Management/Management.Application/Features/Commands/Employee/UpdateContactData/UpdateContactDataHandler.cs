using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;

namespace Management.Application.Features.Commands.Employee.UpdateContactData;

public class UpdateContactDataHandler : ICommandHandler<UpdateContactDataCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateContactDataHandler(IUnitOfWork unitOfWork)
    {
        _employeeRepository = unitOfWork?.EmployeeRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Unit> Handle(UpdateContactDataCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId);
        if (employee == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Employee"),
                Titles.MethodFailedTitle("GetEmployeeByIdAsync"));
        }

        employee.UpdateContact(request.PhoneNumber, request.Email);

        await _employeeRepository.UpdateContactDataAsync(employee);

        return Unit.Value;
    }
}