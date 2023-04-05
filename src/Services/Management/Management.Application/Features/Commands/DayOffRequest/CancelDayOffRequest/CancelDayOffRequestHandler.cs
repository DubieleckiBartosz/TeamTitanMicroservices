using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;

namespace Management.Application.Features.Commands.DayOffRequest.CancelDayOffRequest;

public class CancelDayOffRequestHandler : ICommandHandler<CancelDayOffRequestCommand, Unit>
{
    private readonly IDayOffRequestRepository _dayOffRequestRepository;
    public CancelDayOffRequestHandler(IUnitOfWork unitOfWork)
    {
        _dayOffRequestRepository =
            unitOfWork?.DayOffRequestRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<Unit> Handle(CancelDayOffRequestCommand request, CancellationToken cancellationToken)
    {
        var dayOffRequest = await _dayOffRequestRepository.GetDayOffRequestByIdAsync(request.DayOffRequestId);
        if (dayOffRequest == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("DayOffRequest"),
                Titles.MethodFailedTitle("GetDayOffRequestByIdAsync"));
        }

        dayOffRequest.Cancel();

        await _dayOffRequestRepository.CancelDayOffRequestAsync(dayOffRequest);

        return Unit.Value;
    }
}