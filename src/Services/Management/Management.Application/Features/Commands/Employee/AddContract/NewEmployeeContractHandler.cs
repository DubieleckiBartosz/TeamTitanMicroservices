using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Domain.Entities;
using Management.Domain.Types;
using Management.Domain.ValueObjects;
using MediatR;
using Shared.Domain.Base;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Management.Application.Features.Commands.Employee.AddContract;
 
public class NewEmployeeContractHandler : ICommandHandler<NewEmployeeContractCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IEmployeeRepository _employeeRepository;

    public NewEmployeeContractHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _employeeRepository = unitOfWork.EmployeeRepository;
    }

    public async Task<Unit> Handle(NewEmployeeContractCommand request, CancellationToken cancellationToken)
    { 
        var employee = (await _employeeRepository.GetEmployeeWithDetailsByIdAsync(request.EmployeeId))?.Map();
        if (employee == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Employee"),
                Titles.MethodFailedTitle("GetEmployeeByIdAsync"));
        }

        var position = request.Position;
        var contractType = Enumeration.GetById<ContractType>((int) request.ContractType);
        var settlementType = Enumeration.GetById<SettlementType>((int)request.SettlementType);
        var salary = request.Salary;
        var timeRange = TimeRange.Create(request.StartContract, request.EndContract);
        var numberHoursPerDay = request.NumberHoursPerDay;
        var freeDaysPerYear = request.FreeDaysPerYear;
        var bankAccountNumber = request.BankAccountNumber;
        var paidIntoAccount = request.PaidIntoAccount;
        var hourlyRate = request.HourlyRate;
        var overtimeRate = request.OvertimeRate;
        var paymentMonthDay = request.PaymentMonthDay;
        var userCode = _currentUser.VerificationCode!;

        var newContract = EmployeeContract.Create(position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paidIntoAccount, userCode,
            hourlyRate,
            overtimeRate, paymentMonthDay);

        employee.AddContract(newContract);

        await _employeeRepository.AddContractToEmployeeAsync(employee);
        await _unitOfWork.CompleteAsync(employee);

        return Unit.Value;
    }
}