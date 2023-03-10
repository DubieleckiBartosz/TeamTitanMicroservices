using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.DeactivateAccount;

public class DeactivateAccountHandler : ICommandHandler<DeactivateAccountCommand, Unit>
{
    public Task<Unit> Handle(DeactivateAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}