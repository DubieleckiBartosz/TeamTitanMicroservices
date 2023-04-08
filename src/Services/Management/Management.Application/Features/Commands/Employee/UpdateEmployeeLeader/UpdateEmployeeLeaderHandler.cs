using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;

namespace Management.Application.Features.Commands.Employee.UpdateEmployeeLeader;

public class UpdateEmployeeLeaderHandler : ICommandHandler<UpdateEmployeeLeaderCommand, Unit>
{ 
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeLeaderHandler(IUnitOfWork unitOfWork)
    { 
        _employeeRepository = unitOfWork?.EmployeeRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<Unit> Handle(UpdateEmployeeLeaderCommand request, CancellationToken cancellationToken)
    {
        var employee = (await _employeeRepository.GetEmployeeNecessaryDataByIdAsync(request.EmployeeId))?.Map();
        if (employee == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Employee"),
                Titles.MethodFailedTitle("GetEmployeeNecessaryDataByIdAsync"));
        }

        employee.UpdateLeader(request.NewLeader);

        await _employeeRepository.UpdateLeaderAsync(employee);

        return Unit.Value;
    }
}