using Calculator.Application.Constants;
using Calculator.Application.Features.Account.Commands.AccountSettlement;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators; 
using Shared.Implementations.Background;

namespace Calculator.Application.Features.Account.Commands.CompleteData;

public class CompleteAccountDataHandler : ICommandHandler<CompleteAccountDataCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly IJobService _jobService;

    public CompleteAccountDataHandler(IRepository<Domain.Account.Account> repository, IJobService jobService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
    }

    public async Task<Unit> Handle(CompleteAccountDataCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNull("Account");

        var countingType = request.CountingType;
        var workDayHours = request.WorkDayHours;
        var settlementDayMonth = request.SettlementDayMonth; 
        var expirationDate = request.ExpirationDate;

        account.CompleteAccount(countingType, workDayHours, settlementDayMonth, expirationDate);

        await _repository.UpdateAsync(account);

        var currentDate = DateTime.UtcNow;
        var targetDate = new DateTime(currentDate.Year, currentDate.Month, settlementDayMonth);
        if (targetDate < currentDate)
        {
            targetDate = targetDate.AddMonths(1);
        }

        var timeDifference = targetDate - currentDate;
        var cronExpression =
            $"0 {timeDifference.Minutes} {timeDifference.Hours} {settlementDayMonth} {targetDate.Month} ? {targetDate.Year}";

        _jobService.RecurringMediator(Keys.SettlementBackgroundJobName, new AccountSettlementCommand(account.Id),
            cronExpression);

        return Unit.Value;
    }
}