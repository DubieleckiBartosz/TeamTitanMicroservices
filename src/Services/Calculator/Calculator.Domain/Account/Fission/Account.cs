using Calculator.Domain.Account.Events;
using Shared.Domain.Tools;

namespace Calculator.Domain.Account;

public partial class Account 
{
    public AccountDetails Details { get; private set; }
    public List<ProductItem> ProductItems { get; private set; } = new List<ProductItem>();
    public List<WorkDay> WorkDays { get; private set; } = new List<WorkDay>();
    public List<Settlement> Settlements { get; private set; } = new List<Settlement>();
    public List<Bonus> Bonuses { get; private set; } = new List<Bonus>(); 

    public void Initiated(NewAccountInitiated @event)
    {
        Id = @event.AccountId;
        Details = AccountDetails.Init(@event.AccountCode, @event.DepartmentCode, @event.CreatedBy);
        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }
    internal void DataCompleted(AccountDataCompleted @event)
    {
        Details.AssignData(@event.CountingType, @event.Status, false,
            @event.WorkDayHours, @event.HourlyRate, @event.OvertimeRate, @event.ExpirationDate);
    }

    internal void AccountActivated(AccountActivated @event)
    {
        Details.Activate(@event.ActivatedBy);
    }

    internal void AccountDeactivated(AccountDeactivated @event)
    {
        Details.Deactivate(@event.DeactivatedBy);
    }

    internal void WorkDayHoursUpdated(DayHoursChanged @event)
    {
        Details.UpdateWorkDayHours(@event.NewWorkDayHours);
    }

    internal void HourlyRateUpdated(HourlyRateChanged @event)
    {
        Details.UpdateHourlyRate(@event.NewHourlyRate);
    }

    internal void OvertimeRateUpdated(OvertimeRateChanged @event)
    {
        Details.UpdateOvertimeRate(@event.NewOvertimeRate);
    }

    internal void CountingTypeUpdated(CountingTypeChanged @event)
    {
        Details.UpdateCountingType(@event.NewCountingType);
    }

    internal void NewWorkDayAdded(WorkDayAdded @event)
    {
        var workDay = WorkDay.Create(@event.Date, @event.HoursWorked, @event.Overtime, @event.IsDayOff, @event.CreatedBy);
        WorkDays.Add(workDay);

        if (@event.IsDayOff)
        {
            return;
        }

        Details.IncreaseBalance(@event);
    }

    internal void NewPieceProductItemAdded(PieceProductAdded @event)
    {
        var pieceProduct =
            ProductItem.Create(@event.PieceworkProductId, @event.Quantity, @event.CurrentPrice, @event.Date);

        ProductItems.Add(pieceProduct);

        Details.IncreaseBalance(@event);
    }

    internal void BonusToAccountAdded(BonusAdded @event)
    {
        var newBonus = Bonus.Create(@event.Creator, @event.BonusCode);
        Bonuses!.Add(newBonus);
    }

    internal void AccountBonusCanceled(BonusCanceled @event)
    {
        var bonus = Bonuses?.FirstOrDefault(_ => _.BonusCode == @event.BonusCode);

        if (Bonuses != null && bonus != null)
        {
            Bonuses.Replace(bonus, bonus.AsCanceled());
        }
    }

    internal void Settlement(AccountSettled @event)
    {
        Details.ClearBalance();
    }
}