﻿using Calculator.Application.Constants;
using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Background;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.AccountSettlement;

public class AccountSettlementCommandHandler : ICommandHandler<AccountSettlementCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly IJobService _jobService;

    public AccountSettlementCommandHandler(IRepository<Domain.Account.Account> repository, IJobService jobService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
    }

    public async Task<Unit> Handle(AccountSettlementCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);
         
        account.CheckAndThrowWhenNullOrNotMatch("Account");
        account!.AccountSettlement();

        var expirationDate = account!.Details.ExpirationDate;

        if (!account.IsActive || (expirationDate.HasValue && expirationDate.Value < DateTime.UtcNow))
        {
            //This is the last payment because the account has been deactivated or expiration date is in the past
            var jobName = Keys.SettlementBackgroundJobName(account.Id.ToString());
            _jobService.DeleteBackgroundJobByUniqueJobName(jobName, $"Job removed: {jobName}");

            await _repository.UpdateAsync(account);
        }
        else
        {
            //Otherwise it is normal payout
            await _repository.UpdateWithSnapshotAsync<AccountSnapshot>(account);
        }

        return Unit.Value;
    }
} 