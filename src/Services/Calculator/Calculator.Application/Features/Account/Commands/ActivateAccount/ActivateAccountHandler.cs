using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.ActivateAccount;

public class ActivateAccountHandler : ICommandHandler<ActivateAccountCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ActivateAccountHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public Task<Unit> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}