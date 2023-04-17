using Calculator.Application.Constants;
using Calculator.Application.Features.Account.Commands.AccountSettlement;
using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Background;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.ActivateAccount;

public class ActivateAccountCommandHandler : ICommandHandler<ActivateAccountCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IJobService _jobService;

    public ActivateAccountCommandHandler(IRepository<Domain.Account.Account> repository, ICurrentUser currentUser, IJobService jobService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
    }
    public async Task<Unit> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);

        account.CheckAndThrowWhenNullOrNotMatch("Account", _ => _.Details.CompanyCode == _currentUser.OrganizationCode);

        var userCode = _currentUser.VerificationCode!;
        account!.ActivateAccount(userCode);

        await _repository.UpdateAsync(account);

        var settlementDayMonth = account.Details.SettlementDayMonth;
        var jobName = Keys.SettlementBackgroundJobName(account.Id.ToString());

        if (settlementDayMonth != null && !_jobService.JobExists(jobName))
        {
            var cronExpression = $"0 0 {settlementDayMonth} * *";

            _jobService.RecurringJobMediator(jobName, new AccountSettlementCommand(account.Id),
                cronExpression);
        }
         
        return Unit.Value;
    }
}