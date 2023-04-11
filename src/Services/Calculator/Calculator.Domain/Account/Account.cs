using Calculator.Domain.Account.Events;
using Calculator.Domain.Account.Generators;
using Calculator.Domain.Account.Snapshots;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;
using System.Net;

namespace Calculator.Domain.Account;

public partial class Account : Aggregate
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
    /// <param name="accountOwner"></param>
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
    /// <param name="payoutAmount"></param>
    private Account(Guid accountId, int version, string accountOwner, string companyCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance, DateTime? expirationDate,
        int settlementDayMonth, decimal? payoutAmount)
    {
        Id = accountId;
        Version = version;
        Details = AccountDetails.LoadAccountDetails(
            accountOwner, companyCode,
            countingType, accountStatus, activatedBy, 
            createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate,
            balance, expirationDate, settlementDayMonth, payoutAmount); 

        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }

    public static Account Create(string companyCode, string accountOwnerCode, string createdBy)
    {
        return new Account(accountOwnerCode, companyCode, createdBy);
    }

    public void UpdateAccount(CountingType countingType, int workDayHours, int settlementDayMonth,
        DateTime? expirationDate)
    {
        var @event =
            AccountDataUpdated.Create(countingType, AccountStatus.New, workDayHours, settlementDayMonth, Id,
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
        if (!ProductItems.Any() && !Bonuses.Any() && !WorkDays.Any() && !Details.IsActive)
        {
            //EXCEPTION
        }

        var now = DateTime.UtcNow;
        var currentMonth = now.Month;
        var currentYear = now.Year;
         
        var to = new DateTime(currentYear, currentMonth, (int) Details.SettlementDayMonth!);
        var from = to.AddMonths(-1).AddDays(-1);
        var @event = AccountSettled.Create(this.Id, Details.Balance, from, to);

        Apply(@event);
        this.Enqueue(@event);
    } 
    
    public void UpdateSettlementDay(int newSettlementDay)
    {
        this.ThrowWhenNotActive();

        var @event = SettlementDayMonthUpdated.Create(newSettlementDay, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void AccountUpdateFinancialData(decimal? payoutAmount, decimal? overtimeRate, decimal? hourlyRate)
    {
        this.ThrowWhenNotActive();

        var @event = FinancialDataUpdated.Create(payoutAmount, overtimeRate, hourlyRate, this.Id);
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

    public bool IsActive => Details.IsActive;
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
            details.SettlementDayMonth!.Value,
            details.PayoutAmount);
    }
    protected override void When(IEvent @event)
    {
        switch (@event)
        {
            case NewAccountInitiated e:
                this.Initiated(e);
                break;
            case AccountDataUpdated e:
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
            case WorkDayAdded e:
                this.NewWorkDayAdded(e);
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
            case SettlementDayMonthUpdated e:
                this.AccountSettlementDayMonthUpdated(e);
                break;
            default:
                break;
        }
    } 
}