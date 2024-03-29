﻿using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;

namespace Management.Application.Features.Commands.Employee.UpdateAddressData;

public class UpdateAddressDataHandler : ICommandHandler<UpdateAddressDataCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateAddressDataHandler(IUnitOfWork unitOfWork)
    {
        _employeeRepository = unitOfWork?.EmployeeRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Unit> Handle(UpdateAddressDataCommand request, CancellationToken cancellationToken)
    {
        var employee = (await _employeeRepository.GetEmployeeWithCommunicationDataByIdAsync(request.EmployeeId))?.Map();
        if (employee == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Employee"),
                Titles.MethodFailedTitle("GetEmployeeWithCommunicationDataByIdAsync"));
        }

        employee.UpdateAddress(request.City, request.Street, request.NumberStreet, request.PostalCode);

        await _employeeRepository.UpdateAddressAsync(employee);

        return Unit.Value;
    }
}