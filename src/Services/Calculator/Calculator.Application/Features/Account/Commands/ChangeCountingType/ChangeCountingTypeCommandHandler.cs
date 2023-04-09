using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.ChangeCountingType;

public class ChangeCountingTypeCommandHandler : ICommandHandler<ChangeCountingTypeCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository; 

    public ChangeCountingTypeCommandHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Unit> Handle(ChangeCountingTypeCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);

        account.CheckAndThrowWhenNullOrNotMatch("Account");

        account!.UpdateCountingType(request.NewCountingType);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}