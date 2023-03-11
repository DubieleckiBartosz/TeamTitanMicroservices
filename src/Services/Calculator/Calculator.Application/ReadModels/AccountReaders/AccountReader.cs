using Calculator.Domain.Account.Events;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Implementations.Projection;

namespace Calculator.Application.ReadModels.AccountReaders;

public class AccountReader : IRead
{
    public Guid Id { get; }
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

    internal AccountReader()
    {
    }

    private AccountReader(Guid accountId, string accountOwner, string departmentCode, string createdBy)
    {
        Id = accountId;
        AccountOwner = accountOwner;
        DepartmentCode = departmentCode;
        CreatedBy = createdBy;
    }
    public static AccountReader Create(NewAccountInitiated @event)
    {
        return new AccountReader(@event.AccountId, @event.AccountCode, @event.DepartmentCode, @event.CreatedBy);
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
        var pieceProduct = ProductItemReader.Create(@event.PieceworkProductId, @event.Quantity, @event.CurrentPrice, this.Id);
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