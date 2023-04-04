using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Logging;

namespace Management.Application.Features.Employee.Commands.AssignAccount;

public class AssignCalculationAccountHandler : ICommandHandler<AssignCalculationAccountCommand, Unit>
{
    private readonly ILoggerManager<AssignCalculationAccountHandler> _loggerManager;
    private readonly IEmployeeRepository _employeeRepository;

    public AssignCalculationAccountHandler(IUnitOfWork unitOfWork, ILoggerManager<AssignCalculationAccountHandler> loggerManager)
    {
        _loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
        _employeeRepository = unitOfWork?.EmployeeRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Unit> Handle(AssignCalculationAccountCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByCodeAsync(request.OwnerVerificationCode);
        if (employee == null)
        {
            _loggerManager.LogError(message: $"Account {request.AccountId} has not been assigned");

            throw new NotFoundException(Messages.DataNotFoundMessage("Employee"),
                Titles.MethodFailedTitle("GetEmployeeByIdAsync"));
        }

        employee.AssignAccount(request.AccountId);
        await _employeeRepository.AddAccountToEmployeeAsync(employee);

        return Unit.Value;
    }
}