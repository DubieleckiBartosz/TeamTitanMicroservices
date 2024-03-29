﻿using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.DeactivateAccount;

public class DeactivateAccountCommandHandler : ICommandHandler<DeactivateAccountCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly ICurrentUser _currentUser;

    public DeactivateAccountCommandHandler(IRepository<Domain.Account.Account> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(DeactivateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);
        account.CheckAndThrowWhenNullOrNotMatch("Account", _ => _.Details.CompanyCode == _currentUser.OrganizationCode);

        var userCode = _currentUser.VerificationCode;
        account!.DeactivateAccount(userCode!);
        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}