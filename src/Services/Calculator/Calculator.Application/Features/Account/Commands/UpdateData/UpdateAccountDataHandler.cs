﻿using Calculator.Application.Constants;
using Calculator.Application.Features.Account.Commands.AccountSettlement;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;
using Shared.Implementations.Background;
using Shared.Implementations.Services;

namespace Calculator.Application.Features.Account.Commands.UpdateData;

public class UpdateAccountDataHandler : ICommandHandler<UpdateAccountDataCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly IJobService _jobService;
    private readonly ICurrentUser _currentUser;

    public UpdateAccountDataHandler(IRepository<Domain.Account.Account> repository, IJobService jobService, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<Unit> Handle(UpdateAccountDataCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNullOrNotMatch("Account", _ => _.Details.CompanyCode == _currentUser.OrganizationCode);

        var countingType = request.CountingType;
        var workDayHours = request.WorkDayHours;
        var settlementDayMonth = request.SettlementDayMonth;
        var expirationDate = request.ExpirationDate;

        account.UpdateAccount(countingType, workDayHours, settlementDayMonth, expirationDate);

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

        _jobService.RecurringJobMediator(Keys.SettlementBackgroundJobName(account.Id.ToString()), new AccountSettlementCommand(account.Id),
            cronExpression);

        return Unit.Value;
    }
}