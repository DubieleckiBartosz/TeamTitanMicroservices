using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.AccountSettlement;

public class AccountSettlementCommandHandler : ICommandHandler<AccountSettlementCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public AccountSettlementCommandHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<Unit> Handle(AccountSettlementCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);

        account.CheckAndThrowWhenNullOrNotMatch("Account");
        account!.AccountSettlement(); 
        await _repository.AddWithSnapshotAsync<AccountSnapshot>(account);

        return Unit.Value;
    }
}