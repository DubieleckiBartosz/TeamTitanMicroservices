using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeOvertimeRate;

public class ChangeOvertimeRateHandler : ICommandHandler<ChangeOvertimeRateCommand, Unit>
{
    public Task<Unit> Handle(ChangeOvertimeRateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}