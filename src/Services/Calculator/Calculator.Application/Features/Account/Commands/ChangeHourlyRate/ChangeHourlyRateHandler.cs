using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeHourlyRate;

public class ChangeHourlyRateHandler : ICommandHandler<ChangeHourlyRateCommand, Unit>
{
    public Task<Unit> Handle(ChangeHourlyRateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}