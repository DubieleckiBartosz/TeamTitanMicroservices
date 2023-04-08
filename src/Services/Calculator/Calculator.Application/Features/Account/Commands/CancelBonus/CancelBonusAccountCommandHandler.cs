using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.CancelBonus;

public class CancelBonusAccountCommandHandler : ICommandHandler<CancelBonusAccountCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly ICurrentUser _currentUser;

    public CancelBonusAccountCommandHandler(IRepository<Domain.Account.Account> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<Unit> Handle(CancelBonusAccountCommand request, CancellationToken cancellationToken)
    {
        var bonus = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);

        bonus.CheckAndThrowWhenNullOrNotMatch("Account", _ => _.Details.CompanyCode == _currentUser.OrganizationCode);

        bonus!.CancelBonus(request.BonusCode);
        await _repository.UpdateAsync(bonus);

        return Unit.Value;
    }
}