using Calculator.Domain.Account.Events;
using Shared.Domain.DomainExceptions;
using Shared.Domain.Tools;

namespace Calculator.Domain.Account;
public partial class Account
{
    private void Initiated(NewAccountInitiated @event)
    {
        Id = @event.AccountId;
        Details = AccountDetails.Init(@event.AccountOwnerCode, @event.CompanyCode, @event.CreatedBy);
        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }
    private void DataCompleted(AccountDataUpdated @event)
    {
        Details.AssignData(@event.CountingType, @event.Status, true,
            @event.WorkDayHours, @event.SettlementDayMonth, @event.ExpirationDate);
    }
    private void AccountSettlementDayMonthUpdated(SettlementDayMonthUpdated @event)
    {
        Details.UpdateSettlementDayMonth(@event.SettlementDayMonth);
    }

    public void AccountFinancialDataUpdated(FinancialDataUpdated @event)
    {
        Details.AssignFinancialData(@event.PaymentAmount, @event.HourlyRate, @event.OvertimeRate);
    }


    private void AccountActivated(AccountActivated @event)
    {
        Details.Activate(@event.ActivatedBy);
    }

    private void AccountDeactivated(AccountDeactivated @event)
    {
        Details.Deactivate(@event.DeactivatedBy);
    }

    private void WorkDayHoursUpdated(DayHoursChanged @event)
    {
        Details.UpdateWorkDayHours(@event.NewWorkDayHours);
    }
     
    private void CountingTypeUpdated(CountingTypeChanged @event)
    {
        Details.UpdateCountingType(@event.NewCountingType);
    }

    private void NewWorkDayAdded(WorkDayAdded @event)
    {
        var workDay = WorkDay.Create(@event.Date, @event.HoursWorked, @event.Overtime, @event.IsDayOff, @event.CreatedBy);
        WorkDays.Add(workDay);

        if (@event.IsDayOff)
        {
            return;
        }

        Details.IncreaseBalance(@event);
    }

    private void NewPieceProductItemAdded(PieceProductAdded @event)
    {
        var pieceProduct =
            ProductItem.Create(@event.PieceworkProductId, @event.Quantity, @event.CurrentPrice, @event.Date);

        ProductItems.Add(pieceProduct);

        Details.IncreaseBalance(@event);
    }

    private void BonusToAccountAdded(BonusAdded @event)
    {
        var newBonus = Bonus.Create(@event.Creator, @event.BonusCode, @event.BonusAmount);
        Bonuses!.Add(newBonus);
        Details.IncreaseBalance(@event);
    }

    private void AccountBonusCanceled(BonusCanceled @event)
    {
        var bonus = Bonuses?.FirstOrDefault(_ => _.BonusCode == @event.BonusCode);

        if (bonus != null)
        {
            Details.DecreaseBalance(bonus.Amount);
            Bonuses!.Replace(bonus, bonus.AsCanceled());
        }
    }

    private void Settled(AccountSettled @event)
    {
        var settlement = Settlement.Create(@event.Balance, @event.From, @event.To);

        Settlements.Add(settlement);

        Details.ClearBalance(); 
        var takeFrom = @event.From;

        if (ProductItems.Any())
        {
            ProductItems.RemoveAll(p => p.Date < takeFrom);
            ProductItems.ForEach(_ => _.AsConsidered());
        }

        if (WorkDays.Any())
        {
            WorkDays.RemoveAll(w => w.Date < takeFrom);
        }

        if (Bonuses.Any())
        {
            Bonuses.RemoveAll(b => b.Created < takeFrom);
            Bonuses.ForEach(_ => _.AsSettled());
        }
    }

    private void ThrowWhenNotActive()
    {
        if (!Details.IsActive)
        {
            throw new BusinessException("Incorrect account status", "Account must be active if you want to modify it.");
        }
    }

    private void ThrowWhenDateOfRange(DateTime date)
    {
        var dayMonth = (int)Details.SettlementDayMonth!;
        var currentDate = DateTime.UtcNow;
        var lastMonthEnd = new DateTime(currentDate.Year, currentDate.Month, dayMonth).AddDays(-1);
        var from = new DateTime(currentDate.Year, currentDate.Month, dayMonth).AddMonths(-1);

        if (date < from || date > lastMonthEnd)
        {
            throw new BusinessException("Incorrect date", "Fate is out of range.");
        }
    }
}