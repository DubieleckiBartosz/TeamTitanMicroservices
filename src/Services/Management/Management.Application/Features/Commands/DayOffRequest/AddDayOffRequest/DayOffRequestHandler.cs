using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Domain.Types;
using Management.Domain.ValueObjects;
using MediatR;
using Shared.Domain.Base;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Management.Application.Features.Commands.DayOffRequest.AddDayOffRequest;

public class DayOffRequestHandler : ICommandHandler<DayOffRequestCommand, Unit>
{
    private readonly ICurrentUser _currentUser;
    private readonly IEmployeeRepository _employeeRepository;

    public DayOffRequestHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _employeeRepository = unitOfWork?.EmployeeRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<Unit> Handle(DayOffRequestCommand request, CancellationToken cancellationToken)
    {
        var code = _currentUser.VerificationCode!;
        var employee = await _employeeRepository.GetEmployeeByCodeAsync(code);
        if (employee == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Employee"),
                Titles.MethodFailedTitle("GetEmployeeByCodeAsync"));
        }

        var range = RangeDaysOff.CreateRangeDaysOff(request.From, request.To);
        var reason = Enumeration.GetById<ReasonType>((int)request.ReasonType);
        var description = request.Description != null
            ? DayOffRequestDescription.CreateDescription(request.Description)
            : null;

        var newDayOffRequest = Domain.Entities.DayOffRequest.Create(code, range, reason, description);

        employee.AddDayOffRequest(newDayOffRequest);

        await _employeeRepository.AddDayOffRequestToEmployeeAsync(employee);

        return Unit.Value;
    }
}