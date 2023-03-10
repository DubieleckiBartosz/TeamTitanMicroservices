using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.DeactivateAccount;

public class DeactivateAccountHandler : ICommandHandler<DeactivateAccountCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public DeactivateAccountHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<Unit> Handle(DeactivateAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}