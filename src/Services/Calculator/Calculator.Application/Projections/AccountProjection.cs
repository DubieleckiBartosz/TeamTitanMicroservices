﻿using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Domain.Account.Events;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Projection;

namespace Calculator.Application.Projections;

public class AccountProjection : ReadModelAction<AccountReader>
{
    private readonly IAccountRepository _accountRepository;

    public AccountProjection(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

        this.Projects<AccountActivated>(Handle);
        this.Projects<AccountDataCompleted>(Handle);
        this.Projects<AccountDeactivated>(Handle);
        this.Projects<CountingTypeChanged>(Handle);
        this.Projects<DayHoursChanged>(Handle);
        this.Projects<HourlyRateChanged>(Handle);
        this.Projects<NewAccountInitiated>(Handle);
        this.Projects<OvertimeRateChanged>(Handle);
        this.Projects<PieceProductAdded>(Handle);
        this.Projects<WorkDayAdded>(Handle);
        this.Projects<BonusAdded>(Handle);
        this.Projects<BonusCanceled>(Handle);
        this.Projects<FinancialDataUpdated>(Handle);
        this.Projects<AccountSettled>(Handle);
    }

    private async Task Handle(AccountActivated @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.AccountActivated(@event);
        await _accountRepository.UpdateStatusToActiveAsync(account); 
    }

    private async Task Handle(AccountSettled @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountWithProductsAndBonusesByIdAsync(@event.AccountId, @event.From, @event.To);
        this.CheckAccount(account);

        account!.Settled(@event);
        await _accountRepository.AddSettlementAsync(account);
    }


    private async Task Handle(AccountDataCompleted @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.DataCompleted(@event);
        await _accountRepository.UpdateDataAsync(account);
    }
    
    private async Task Handle(FinancialDataUpdated @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.AssignFinancialData(@event);
        await _accountRepository.UpdateFinancialDataAsync(account);
    }

    private async Task Handle(AccountDeactivated @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.AccountDeactivated(@event);
        await _accountRepository.UpdateStatusToDeactivateAsync(account);
    }

    private async Task Handle(CountingTypeChanged @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.CountingTypeUpdated(@event);
        await _accountRepository.UpdateCountingTypeAsync(account);
    }

    private async Task Handle(DayHoursChanged @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.WorkDayHoursUpdated(@event);
        await _accountRepository.UpdateWorkDayHoursAsync(account);
    }

    private async Task Handle(HourlyRateChanged @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.HourlyRateUpdated(@event);
        await _accountRepository.UpdateHourlyRateAsync(account);
    }

    private async Task Handle(NewAccountInitiated @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var newAccount = AccountReader.Create(@event);
        await _accountRepository.AddAsync(newAccount);
    }

    private async Task Handle(OvertimeRateChanged @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.OvertimeRateUpdated(@event);
        await _accountRepository.UpdateOvertimeRateAsync(account);
    }

    private async Task Handle(PieceProductAdded @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.NewPieceProductItemAdded(@event);
        await _accountRepository.AddProductItemAsync(account);
    }

    private async Task Handle(WorkDayAdded @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdAsync(@event.AccountId);

        this.CheckAccount(account);

        account!.NewWorkDayAdded(@event);
        await _accountRepository.AddNewWorkDayAsync(account);
    }

    public async Task Handle(BonusAdded @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdWithBonusesAsync(@event.AccountId);
        this.CheckAccount(account);

        account!.BonusToAccountAdded(@event);
        await _accountRepository.AddBonusAsync(account);
    }

    public async Task Handle(BonusCanceled @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var account = await _accountRepository.GetAccountByIdWithBonusesAsync(@event.AccountId);
        this.CheckAccount(account);

        var bonusCanceled = account!.AccountBonusCanceled(@event);
        await _accountRepository.UpdateBonusAccountAsync(bonusCanceled, account);
    }

    private void CheckAccount(AccountReader? account)
    { 
        if (account == null)
        {
            throw new NotFoundException("Account cannot be null.", "Account not found");
        } 
    }
}