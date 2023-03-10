using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeCountingType;

public class ChangeCountingTypeHandler : ICommandHandler<ChangeCountingTypeCommand, Unit>
{
    public Task<Unit> Handle(ChangeCountingTypeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}