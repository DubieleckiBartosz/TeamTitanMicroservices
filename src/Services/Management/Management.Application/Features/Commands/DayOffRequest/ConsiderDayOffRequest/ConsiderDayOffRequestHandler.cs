using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Domain.Statuses;
using MediatR;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Management.Application.Features.Commands.DayOffRequest.ConsiderDayOffRequest;

public class ConsiderDayOffRequestHandler : ICommandHandler<ConsiderDayOffRequestCommand, Unit>
{
    private readonly ICurrentUser _currentUser;
    private readonly IDayOffRequestRepository _dayOffRequestRepository;
    public ConsiderDayOffRequestHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _dayOffRequestRepository =
            unitOfWork?.DayOffRequestRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<Unit> Handle(ConsiderDayOffRequestCommand request, CancellationToken cancellationToken)
    {
        var dayOffRequest = (await _dayOffRequestRepository.GetDayOffRequestByIdAsync(request.DayOffRequestId))?.Map();
        if (dayOffRequest == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("DayOffRequest"),
                Titles.MethodFailedTitle("GetDayOffRequestByIdAsync"));
        }

        var newStatus = request.Positive ? DayOffRequestCurrentStatus.Accepted : DayOffRequestCurrentStatus.Rejected;
        var code = _currentUser.VerificationCode!;
        dayOffRequest.UpdateStatus(code, newStatus);

        await _dayOffRequestRepository.ConsiderDayOffRequestAsync(dayOffRequest);

        return Unit.Value;
    }
}