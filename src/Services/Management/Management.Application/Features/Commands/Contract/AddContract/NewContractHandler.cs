﻿using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Domain.Types;
using Management.Domain.ValueObjects;
using MediatR;
using Shared.Domain.Base;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Management.Application.Features.Commands.Contract.AddContract;

public class NewContractHandler : ICommandHandler<NewContractCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IEmployeeRepository _employeeRepository;

    public NewContractHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _employeeRepository = unitOfWork.EmployeeRepository;
    }

    public async Task<Unit> Handle(NewContractCommand request, CancellationToken cancellationToken)
    {
        var employee = (await _employeeRepository.GetEmployeeWithContractsByIdAsync(request.EmployeeId))?.Map();
        if (employee == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Employee"),
                Titles.MethodFailedTitle("GetEmployeeWithContractsByIdAsync"));
        }

        var position = request.Position;
        var contractType = Enumeration.GetById<ContractType>((int)request.ContractType);
        var settlementType = Enumeration.GetById<SettlementType>((int)request.SettlementType); 
        var timeRange = TimeRange.Create(request.StartContract, request.EndContract);
        var numberHoursPerDay = request.NumberHoursPerDay;
        var freeDaysPerYear = request.FreeDaysPerYear;
        var bankAccountNumber = request.BankAccountNumber;  
        var paymentMonthDay = request.PaymentMonthDay;
        var userCode = _currentUser.VerificationCode!;

        var newContract = Domain.Entities.Contract.Create(position, contractType, settlementType, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, userCode, paymentMonthDay);

        employee.AddContract(newContract);

        await _employeeRepository.AddContractToEmployeeAsync(employee);
        await _unitOfWork.CompleteAsync(employee);

        return Unit.Value;
    }
}