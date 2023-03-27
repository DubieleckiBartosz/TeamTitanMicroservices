using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.ChangeDayHours;

public class ChangeDayHoursCommandHandler : ICommandHandler<ChangeDayHoursCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly ICurrentUser _currentUser;

    public ChangeDayHoursCommandHandler(IRepository<Domain.Account.Account> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<Unit> Handle(ChangeDayHoursCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);

        account.CheckAndThrowWhenNullOrNotMatch("Account", _ => _.Details.CompanyCode == _currentUser.OrganizationCode);

        account.UpdateWorkDayHours(request.NewWorkDayHours);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}