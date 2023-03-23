using Calculator.Domain.Account.Events;
using Calculator.Domain.Account.Generators;
using Calculator.Domain.Account.Snapshots;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;
using System.Net;
using Shared.Domain.Tools;

namespace Calculator.Domain.Account;

public class Account : Aggregate
{
    public AccountDetails Details { get; private set; }
    public List<ProductItem> ProductItems { get; private set; } = new List<ProductItem>();
    public List<WorkDay> WorkDays { get; private set; } = new List<WorkDay>();
    public List<Settlement> Settlements { get; private set; } = new List<Settlement>();
    public List<Bonus> Bonuses { get; private set; } = new List<Bonus>();

    /// <summary>
    /// Constructor for serializer
    /// </summary>
    public Account()
    {
    }

    /// <summary>
    /// Init account
    /// </summary>
    /// <param name="accountOwnerCode"></param>
    /// <param name="companyCode"></param>
    /// <param name="creator"></param>
    private Account(string accountOwnerCode, string companyCode, string creator)
    { 
        var @event = NewAccountInitiated.Create(companyCode, accountOwnerCode, creator, Guid.NewGuid());
        Apply(@event);
        this.Enqueue(@event);
    }

    /// <summary>
    /// Load e.g. from snapshot 
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="version"></param>
    /// <param name="accountOwnerExternalId"></param>
    /// <param name="companyCode"></param>
    /// <param name="countingType"></param>
    /// <param name="accountStatus"></param>
    /// <param name="activatedBy"></param>
    /// <param name="createdBy"></param>
    /// <param name="deactivatedBy"></param>
    /// <param name="isActive"></param>
    /// <param name="workDayHours"></param>
    /// <param name="hourlyRate"></param>
    /// <param name="overtimeRate"></param>
    /// <param name="balance"></param>
    /// <param name="expirationDate"></param>
    /// <param name="settlementDayMonth"></param>
    private Account(Guid accountId, int version, string accountOwnerExternalId, string companyCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance, DateTime? expirationDate, int settlementDayMonth)
    {
        Id = accountId;
        Version = version;
        Details = AccountDetails.LoadAccountDetails(
            accountOwnerExternalId, companyCode,
            countingType, accountStatus, activatedBy, 
            createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate, balance, expirationDate, settlementDayMonth); 

        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }

    public static Account Create(string companyCode, string accountOwnerCode, string createdBy)
    {
        return new Account(accountOwnerCode, companyCode, createdBy);
    }

    public void CompleteAccount(CountingType countingType, int workDayHours, int settlementDayMonth,
        DateTime? expirationDate)
    {
        var @event =
            AccountDataCompleted.Create(countingType, AccountStatus.New, workDayHours, settlementDayMonth, Id,
                expirationDate);
        Apply(@event);
        Enqueue(@event);
    }

    public void ActiveAccount(string activateBy)
    {
        if (Details.IsActive)
        {
            throw new BusinessException("Bad current status", "The account is already activated.");
        }

        var @event =
            Events.AccountActivated.Create(activateBy, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void DeactivateAccount(string deactivateBy)
    {
        if (!Details.IsActive)
        {
            throw new BusinessException("Bad current status", "The account is already deactivated.");
        }

        var @event = Events.AccountDeactivated.Create(deactivateBy, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void UpdateWorkDayHours(int newWorkDayHours)
    {
        this.ThrowWhenNotActive();

        var @event = DayHoursChanged.Create(newWorkDayHours, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void UpdateCountingType(CountingType newCountingType)
    {
        this.ThrowWhenNotActive();

        var @event = CountingTypeChanged.Create(newCountingType, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void AddNewWorkDay(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy)
    {
        this.ThrowWhenNotActive();

        if (hoursWorked > Details.WorkDayHours)
        {
            throw new BusinessException("Incorrect number of hours",
                "The maximum number of hours for this account is 8. The rest must be registered as overtime.");
        }

        this.ThrowWhenDateOfRange(date);

        var @event = WorkDayAdded.Create(date, hoursWorked, overtime, isDayOff, createdBy, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void AddNewPieceProductItem(Guid pieceworkProductId, decimal quantity, decimal currentPrice, DateTime date)
    {
        this.ThrowWhenNotActive();
        this.ThrowWhenDateOfRange(date); 

        var @event = PieceProductAdded.Create(pieceworkProductId, quantity, currentPrice, this.Id, date);
        Apply(@event);
        this.Enqueue(@event);
    }
    
    public void AccountSettlement()
    { 
        var @event = AccountSettled.Create(this.Id, Details.Balance);
        Apply(@event);
        this.Enqueue(@event);
    }
    public void UpdateHourlyRate(decimal newHourlyRate)
    {
        this.ThrowWhenNotActive();

        var @event = HourlyRateChanged.Create(newHourlyRate, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void UpdateOvertimeRate(decimal newOvertimeRate)
    {
        this.ThrowWhenNotActive();

        var @event = OvertimeRateChanged.Create(newOvertimeRate, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void AccountUpdateFinancialData(decimal? overtimeRate, decimal? hourlyRate)
    {
        this.ThrowWhenNotActive();

        var @event = FinancialDataUpdated.Create(overtimeRate, hourlyRate, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void AddBonus(string creator, decimal amount)
    {
        this.ThrowWhenNotActive();

        var repeat = true;
        var bonusCode = string.Empty;
        while (repeat)
        {
            bonusCode = BonusCodeGenerator.GenerateBonusCode(Id.ToString(),"ACC");
            var bonus = Bonuses?.FirstOrDefault(_ => _.BonusCode == bonusCode);

            if (bonus == null)
            {
                repeat = false;
            }
        }

        var @event = BonusAdded.Create(amount, creator, Id, bonusCode);
        Apply(@event);
        Enqueue(@event);
    }

    public void CancelBonus(string bonusCode)
    {
        this.ThrowWhenNotActive();
        if (Bonuses == null)
        {
            throw new BusinessException("List is NULL", "List of bonuses is NULL.",
                HttpStatusCode.NotFound);
        }

        var bonus = Bonuses.FirstOrDefault(_ => _.BonusCode == bonusCode);
        if (bonus == null)
        {
            throw new BusinessException("Bonus is NULL", "Bonus not found in account.",
                HttpStatusCode.NotFound);
        }

        var @event = BonusCanceled.Create(this.Id, bonusCode);

        Apply(@event);
        this.Enqueue(@event);
    }

    public override AccountSnapshot CreateSnapshot()
    {
        return AccountSnapshot.Create(this.Id, this.Version).Set(this);
    }

    public override Account? FromSnapshot(ISnapshot? snapshot)
    {
        var snapshotAccount = (AccountSnapshot?)snapshot;
        if (snapshotAccount?.State?.Details == null)
        {
            return null;
        }

        var details = snapshotAccount.State.Details;
        return new Account(
            snapshotAccount.AccountId,
            snapshotAccount.Version,
            details.AccountOwner,
            details.CompanyCode,
            details.CountingType, 
            details.AccountStatus, 
            details.ActivatedBy,
            details.CreatedBy!, 
            details.DeactivatedBy,
            details.IsActive,
            details.WorkDayHours, 
            details.HourlyRate,
            details.OvertimeRate,
            details.Balance,
            details.ExpirationDate,
            details.SettlementDayMonth!.Value);
    }
    protected override void When(IEvent @event)
    {
        switch (@event)
        {
            case NewAccountInitiated e:
                this.Initiated(e);
                break;
            case AccountDataCompleted e:
                this.DataCompleted(e);
                break;
            case FinancialDataUpdated e:
                this.AccountFinancialDataUpdated(e);
                break;
            case AccountActivated e:
                this.AccountActivated(e);
                break;
            case AccountDeactivated e:
                this.AccountDeactivated(e);
                break;
            case HourlyRateChanged e:
                this.HourlyRateUpdated(e);
                break;
            case WorkDayAdded e:
                this.NewWorkDayAdded(e);
                break;
            case OvertimeRateChanged e:
                this.OvertimeRateUpdated(e);
                break;
            case PieceProductAdded e:
                this.NewPieceProductItemAdded(e);
                break;
            case DayHoursChanged e:
                this.WorkDayHoursUpdated(e);
                break;
            case CountingTypeChanged e:
                this.CountingTypeUpdated(e);
                break;
            case AccountSettled e:
                this.Settled(e);
                break;
            case BonusAdded e:
                this.BonusToAccountAdded(e);
                break;
            case BonusCanceled e:
                this.AccountBonusCanceled(e);
                break;
            default:
                break;
        }
    }
    public void Initiated(NewAccountInitiated @event)
    {
        Id = @event.AccountId;
        Details = AccountDetails.Init(@event.AccountCode, @event.CompanyCode, @event.CreatedBy);
        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }
    private void DataCompleted(AccountDataCompleted @event)
    {
        Details.AssignData(@event.CountingType, @event.Status, false,
            @event.WorkDayHours, @event.SettlementDayMonth, @event.ExpirationDate);
    }

    public void AccountFinancialDataUpdated(FinancialDataUpdated @event)
    {
        Details.AssignFinancialData(@event.HourlyRate, @event.OvertimeRate);
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

    private void HourlyRateUpdated(HourlyRateChanged @event)
    {
        Details.UpdateHourlyRate(@event.NewHourlyRate);
    }

    private void OvertimeRateUpdated(OvertimeRateChanged @event)
    {
        Details.UpdateOvertimeRate(@event.NewOvertimeRate);
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
        var settlement = Settlement.Create(@event.Balance, Details.SettlementDayMonth!.Value);
        
        Settlements.Add(settlement);

        Details.ClearBalance();

        var currentDate = DateTime.UtcNow;
        var dayMonth = (int)Details.SettlementDayMonth!;
        var takeFrom = new DateTime(currentDate.Year, currentDate.Month, dayMonth).AddMonths(-1);

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
            throw new BusinessException("Incorrect account status", "Account must be active if you want to modify it. ");
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