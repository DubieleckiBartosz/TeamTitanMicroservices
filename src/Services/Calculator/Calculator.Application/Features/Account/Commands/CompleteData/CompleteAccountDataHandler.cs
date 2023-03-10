using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.CompleteData;

public class CompleteAccountDataHandler : ICommandHandler<CompleteAccountDataCommand, Unit>
{
    public Task<Unit> Handle(CompleteAccountDataCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}