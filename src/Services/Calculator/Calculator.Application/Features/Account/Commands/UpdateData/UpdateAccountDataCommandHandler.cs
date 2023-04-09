using Calculator.Application.Constants;
using Calculator.Application.Features.Account.Commands.AccountSettlement;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;
using Shared.Implementations.Background;
using Calculator.Domain.Account.Snapshots;

namespace Calculator.Application.Features.Account.Commands.UpdateData;

public class UpdateAccountDataCommandHandler : ICommandHandler<UpdateAccountDataCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly IJobService _jobService; 

    public UpdateAccountDataCommandHandler(IRepository<Domain.Account.Account> repository, IJobService jobService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService)); 
    }

    public async Task<Unit> Handle(UpdateAccountDataCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);

        account.CheckAndThrowWhenNullOrNotMatch("Account");

        var countingType = request.CountingType;
        var workDayHours = request.WorkDayHours;
        var settlementDayMonth = request.SettlementDayMonth;
        var expirationDate = request.ExpirationDate;

        var currentSettlementDay = account!.Details.SettlementDayMonth;
        account!.UpdateAccount(countingType, workDayHours, settlementDayMonth, expirationDate);

        await _repository.UpdateAsync(account);

        if (currentSettlementDay == null || currentSettlementDay != request.SettlementDayMonth)
        {
            var cronExpression = $"0 0 {settlementDayMonth} * *";

            _jobService.RecurringJobMediator(Keys.SettlementBackgroundJobName(account.Id.ToString()), new AccountSettlementCommand(account.Id),
                cronExpression); 
        } 

        return Unit.Value;
    }
}