using Calculator.Domain.Account.Events;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Implementations.Projection;

namespace Calculator.Application.ReadModels.AccountReaders;

public class AccountReader : IRead
{
    public Guid Id { get; }
    public decimal Balance { get; init; }
    public string AccountOwner { get; }
    public string DepartmentCode { get; }
    public CountingType CountingType { get; private set; }
    public AccountStatus AccountStatus { get; private set; }
    public string? ActivatedBy { get; private set; }
    public string CreatedBy { get; }
    public string? DeactivatedBy { get; private set; }
    public bool IsActive { get; private set; }
    public int WorkDayHours { get; private set; }
    public decimal? HourlyRate { get; private set; }
    public decimal? OvertimeRate { get; private set; }
    public List<ProductItemReader> ProductItems { get; private set; } = new List<ProductItemReader>();
    public List<WorkDayReader> WorkDays { get; private set; } = new List<WorkDayReader>();
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
    /// <param name="departmentCode"></param>
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
    /// <param name="productItems"></param>
    /// <param name="workDays"></param>
    private AccountReader(Guid id, string accountOwner, string departmentCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance,
        List<ProductItemReader> productItems, List<WorkDayReader> workDays)
    {
        Id = id;
        AccountOwner = accountOwner;
        DepartmentCode = departmentCode;
        CountingType = countingType;
        AccountStatus = accountStatus;
        ActivatedBy = activatedBy;
        CreatedBy = createdBy;
        DeactivatedBy = deactivatedBy;
        IsActive = isActive;
        WorkDayHours = workDayHours;
        HourlyRate = hourlyRate;
        OvertimeRate = overtimeRate;
        Balance = balance;
        ProductItems = productItems;
        WorkDays = workDays;
    }

    /// <summary>
    /// For logic
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="accountOwner"></param>
    /// <param name="departmentCode"></param>
    /// <param name="createdBy"></param>
    private AccountReader(Guid accountId, string accountOwner, string departmentCode, string createdBy)
    {
        Id = accountId;
        AccountOwner = accountOwner;
        DepartmentCode = departmentCode;
        CreatedBy = createdBy;
        IsActive = false;
        Balance = 0;
    }

    public static AccountReader Create(NewAccountInitiated @event)
    {
        return new AccountReader(@event.AccountId, @event.AccountCode, @event.DepartmentCode, @event.CreatedBy);
    }

    public static AccountReader Load(Guid id, string accountOwner, string departmentCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance, List<ProductItemReader> productItems,
        List<WorkDayReader> workDays)
    {
        return new AccountReader(id, accountOwner, departmentCode, countingType,
            accountStatus, activatedBy, createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate, balance, productItems, workDays);
    }

    public AccountReader DataCompleted(AccountDataCompleted @event)
    {
        CountingType = @event.CountingType;
        AccountStatus = @event.Status;
        IsActive = false;
        WorkDayHours = @event.WorkDayHours;
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

    public AccountReader HourlyRateUpdated(HourlyRateChanged @event)
    {
        HourlyRate = @event.NewHourlyRate;

        return this;
    }

    public AccountReader OvertimeRateUpdated(OvertimeRateChanged @event)
    {
        OvertimeRate = @event.NewOvertimeRate;

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

        return this;
    }

    public AccountReader NewPieceProductItemAdded(PieceProductAdded @event)
    {
        var pieceProduct = ProductItemReader.Create(@event.PieceworkProductId, @event.Quantity, @event.CurrentPrice, Id,
            @event.Date);
        ProductItems.Add(pieceProduct);

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