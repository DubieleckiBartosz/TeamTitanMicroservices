using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

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
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);
        account.CheckAndThrowWhenNull("Account"); 

        account!.DeactivateAccount(request.DeactivateBy);
        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}