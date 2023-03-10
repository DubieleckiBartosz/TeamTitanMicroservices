using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeDayHours;

public class ChangeDayHoursHandler : ICommandHandler<ChangeDayHoursCommand, Unit>
{
    public Task<Unit> Handle(ChangeDayHoursCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}