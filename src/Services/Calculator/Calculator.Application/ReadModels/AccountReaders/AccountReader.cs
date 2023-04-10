using Calculator.Domain.Account.Events;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Tools;
using Shared.Implementations.Projection;

namespace Calculator.Application.ReadModels.AccountReaders;

public class AccountReader : IRead
{
    public Guid Id { get; }
    public decimal Balance { get; private set; }
    public string AccountOwner { get; }
    public string CompanyCode { get; }
    public CountingType CountingType { get; private set; }
    public AccountStatus AccountStatus { get; private set; }
    public string? ActivatedBy { get; private set; }
    public string CreatedBy { get; }
    public string? DeactivatedBy { get; private set; }
    public bool IsActive { get; private set; }
    public int WorkDayHours { get; private set; }
    public decimal? HourlyRate { get; private set; }
    public decimal? OvertimeRate { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public int? SettlementDayMonth { get; private set; }
    public decimal? PayoutAmount { get; private set; }

    public List<ProductItemReader> ProductItems { get; private set; } = new List<ProductItemReader>();
    public List<WorkDayReader> WorkDays { get; private set; } = new List<WorkDayReader>();
    public List<BonusReader> Bonuses { get; private set; } = new List<BonusReader>();
    public List<SettlementReader> Settlements { get; private set; } = new List<SettlementReader>();

    /// <summary>
    /// For instance creation
    /// </summary>
    internal AccountReader()
    {
    }

    /// <summary>
    /// For load
    /// </summary>
    /// <param name="id"></param>
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
    /// <param name="productItems"></param>
    /// <param name="workDays"></param>
    /// <param name="bonuses"></param>
    /// <param name="settlements"></param>
    private AccountReader(Guid id, string accountOwner, string companyCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, 
        decimal balance, DateTime? expirationDate, int? settlementDayMonth, decimal? payoutAmount,
        List<ProductItemReader>? productItems, List<WorkDayReader>? workDays, 
        List<BonusReader>? bonuses, List<SettlementReader>? settlements)
    {
        this.Id = id;
        this.AccountOwner = accountOwner;
        this.CompanyCode = companyCode;
        this.CountingType = countingType;
        this.AccountStatus = accountStatus;
        this.ActivatedBy = activatedBy;
        this.CreatedBy = createdBy;
        this.DeactivatedBy = deactivatedBy;
        this.IsActive = isActive;
        this.WorkDayHours = workDayHours;
        this.HourlyRate = hourlyRate;
        this.OvertimeRate = overtimeRate;
        this.Balance = balance;
        this.PayoutAmount = payoutAmount;
        this.ProductItems = productItems ?? new();
        this.WorkDays = workDays ?? new();
        this.Bonuses = bonuses ?? new();
        this.ExpirationDate = expirationDate;
        this.SettlementDayMonth = settlementDayMonth;
        this.Settlements = settlements ?? new();
    }

    /// <summary>
    /// For logic
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="accountOwner"></param>
    /// <param name="companyCode"></param>
    /// <param name="createdBy"></param>
    private AccountReader(Guid accountId, string accountOwner, string companyCode, string createdBy)
    {
        this.Id = accountId;
        this.AccountOwner = accountOwner;
        this.CompanyCode = companyCode;
        this.CreatedBy = createdBy;
        this.IsActive = false;
        this.Balance = 0;
    }

    public static AccountReader Create(NewAccountInitiated @event)
    {
        return new AccountReader(@event.AccountId, @event.AccountOwnerCode, @event.CompanyCode, @event.CreatedBy);
    }

    public static AccountReader Load(Guid id, string accountOwner, string companyCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance, DateTime? expirationDate,
        int? settlementDayMonth, decimal? payoutAmount, List<ProductItemReader> productItems, 
        List<WorkDayReader>? workDays, List<BonusReader>? bonuses, List<SettlementReader>? settlements)
    {
        return new AccountReader(id, accountOwner, companyCode, countingType,
            accountStatus, activatedBy, createdBy, deactivatedBy, isActive, 
            workDayHours, hourlyRate, overtimeRate, balance,
            expirationDate, settlementDayMonth, payoutAmount,
            productItems, workDays, bonuses, settlements);
    }

    public AccountReader DataCompleted(AccountDataUpdated @event)
    {
        CountingType = @event.CountingType;
        AccountStatus = @event.Status;
        IsActive = false;
        WorkDayHours = @event.WorkDayHours;
        SettlementDayMonth = @event.SettlementDayMonth; 
        ExpirationDate = @event.ExpirationDate;
        PayoutAmount = @event.PayoutAmount;

        return this;
    }

    public AccountReader AssignFinancialData(FinancialDataUpdated @event)
    { 
        HourlyRate = @event.HourlyRate;
        OvertimeRate = @event.OvertimeRate;

        return this;
    }

    public AccountReader AccountActivated(AccountActivated @event)
    {
        ActivatedBy = @event.ActivatedBy;
        AccountStatus = AccountStatus.InUse;
        IsActive = true;

        return this;
    }

    public AccountReader AccountDeactivated(AccountDeactivated @event)
    {
        DeactivatedBy = @event.DeactivatedBy;
        AccountStatus = AccountStatus.Off;
        IsActive = false;

        return this;
    }

    public AccountReader WorkDayHoursUpdated(DayHoursChanged @event)
    {
        WorkDayHours = @event.NewWorkDayHours;

        return this;
    }
     
    public AccountReader CountingTypeUpdated(CountingTypeChanged @event)
    {
        CountingType = @event.NewCountingType;

        return this;
    }

    public AccountReader NewWorkDayAdded(WorkDayAdded @event)
    {
        var workDay = WorkDayReader.Create(@event.Date, @event.HoursWorked, @event.Overtime, @event.IsDayOff,
            @event.CreatedBy, this.Id);

        WorkDays.Add(workDay);


        if (!@event.IsDayOff && CountingType == CountingType.ForAnHour)
        {
            Balance = workDay.HoursWorked * HourlyRate!.Value + workDay.Overtime * (HourlyRate ?? 0); 
        }

        return this;
    }

    public AccountReader NewPieceProductItemAdded(PieceProductAdded @event)
    {
        var pieceProduct = ProductItemReader.Create(@event.PieceworkProductId, @event.Quantity, @event.CurrentPrice, Id,
            @event.Date);

        ProductItems.Add(pieceProduct);

        if (CountingType == CountingType.Piecework)
        {
            Balance = @event.Quantity * @event.CurrentPrice; 
        }

        return this;
    }

    public void BonusToAccountAdded(BonusAdded @event)
    {
        var newBonus = BonusReader.Create(@event.Creator, @event.BonusCode, @event.BonusAmount);
        Bonuses!.Add(newBonus);
        Balance = @event.BonusAmount;
    }

    public BonusReader AccountBonusCanceled(BonusCanceled @event)
    {
        var bonus = Bonuses.First(_ => _.BonusCode == @event.BonusCode); 
        Balance -= bonus.Amount;
        Bonuses.Replace(bonus, bonus.AsCanceled());

        return bonus;
    }

    public AccountReader Settled(AccountSettled @event)
    {
        var settlement = SettlementReader.Create(@event.Balance, @event.From, @event.To);
  
        Settlements.Add(settlement);
        Balance = 0; 
          
        if (ProductItems.Any())
        {
             ProductItems.ForEach(_ => _.AsConsidered());
        } 

        if (Bonuses.Any())
        { 
            Bonuses.ForEach(_ => _.AsSettled());
        }

        return this;
    }


    public AccountReader UpdateSettlementDayMonth(SettlementDayMonthUpdated @event)
    {
        SettlementDayMonth = @event.SettlementDayMonth;

        return this;
    }

    public ProductItemReader? GetLastProductItem()
    {
        return this.ProductItems.LastOrDefault();
    }
    public WorkDayReader? GetLastWorkDay()
    {
        return this.WorkDays.LastOrDefault();
    }
}