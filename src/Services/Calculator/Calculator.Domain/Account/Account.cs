using Calculator.Domain.Account.Events;
using Calculator.Domain.Account.Snapshots;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.Account;

public class Account : Aggregate
{
    public AccountDetails Details { get; private set; }
    public List<ProductItem> ProductItems { get; private set; } = new List<ProductItem>();
    public List<WorkDay> WorkDays { get; private set; } = new List<WorkDay>();

    /// <summary>
    /// Init account
    /// </summary>
    /// <param name="accountOwnerExternalId"></param>
    /// <param name="departmentCode"></param>
    /// <param name="creator"></param>
    private Account(string accountOwnerExternalId, string departmentCode, string creator)
    { 
        var @event = NewAccountInitiated.Create(departmentCode, accountOwnerExternalId, creator, Guid.NewGuid());
        Apply(@event);
        this.Enqueue(@event);
    }

    /// <summary>
    /// Load e.g. from snapshot 
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="version"></param>
    /// <param name="accountOwnerExternalId"></param>
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
    private Account(Guid accountId, int version, string accountOwnerExternalId, string departmentCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate)
    {
        Id = accountId;
        Version = version;
        Details = AccountDetails.LoadAccountDetails(
            accountOwnerExternalId, departmentCode,
            countingType, accountStatus, activatedBy, 
            createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate); 

        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }

    public static Account Create(string departmentCode, string accountOwnerExternalId, string createdBy)
    {
        return new Account(accountOwnerExternalId, departmentCode, createdBy);
    }

    public void CompleteAccount(CountingType countingType, int workDayHours, decimal? overtimeRate, decimal? hourlyRate)
    {
        var @event =
            AccountDataCompleted.Create(countingType, AccountStatus.New, workDayHours, overtimeRate, hourlyRate, this.Id);
        Apply(@event);
        this.Enqueue(@event); 
    }
    public void ActiveAccount(string activateBy)
    {
        var @event =
            Events.AccountActivated.Create(activateBy, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void DeactivateAccount(string deactivateBy)
    {
        var @event = Events.AccountDeactivated.Create(deactivateBy, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void UpdateWorkDayHours(int newWorkDayHours)
    {
        var @event = DayHoursChanged.Create(newWorkDayHours, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void UpdateHourlyRate(decimal newHourlyRate)
    {
        var @event = HourlyRateChanged.Create(newHourlyRate, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }
    
    public void UpdateOvertimeRate(decimal newOvertimeRate)
    {
        var @event = OvertimeRateChanged.Create(newOvertimeRate, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }
    
    public void UpdateCountingType(CountingType newCountingType)
    {
        var @event = CountingTypeChanged.Create(newCountingType, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void AddNewWorkDay(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy)
    {
        var @event = WorkDayAdded.Create(date, hoursWorked, overtime, isDayOff, createdBy, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void AddNewPieceProductItem(Guid pieceworkProductId, decimal quantity, decimal currentPrice)
    {
        var @event = PieceProductAdded.Create(pieceworkProductId, quantity, currentPrice, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public AccountSnapshot CreateSnapshot()
    {
        return AccountSnapshot.Create(this.Id, this.Version).Set(this);
    }
    public Account? FromSnapshot(ISnapshot? snapshot)
    {
        var snapshotAccount = (AccountSnapshot?)snapshot;
        if (snapshotAccount == null || snapshotAccount.State == null || snapshotAccount.State?.Details == null)
        {
            return null;
        }

        var details = snapshotAccount.State.Details;
        return new Account(
            snapshotAccount.AccountId,
            snapshotAccount.Version,
            details.AccountOwnerExternalId,
            details.DepartmentCode,
            details.CountingType, 
            details.AccountStatus, 
            details.ActivatedBy,
            details.CreatedBy!, 
            details.DeactivatedBy,
            details.IsActive,
            details.WorkDayHours, 
            details.HourlyRate,
            details.OvertimeRate);
    }
    protected override void When(IEvent @event)
    {
        switch (@event)
        {
            case NewAccountInitiated e:
                break;
            case AccountDataCompleted e:
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
            default:
                break;
        }
    }
    private void Initiated(NewAccountInitiated @event)
    {
        Id = @event.AccountId;
        Details = AccountDetails.Init(@event.AccountOwnerExternalId, @event.DepartmentCode, @event.CreatedBy);
        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }
    private void DataCompleted(AccountDataCompleted @event)
    {
        Details.AssignData(@event.CountingType, @event.Status, null, null, false,
            @event.WorkDayHours, @event.HourlyRate, @event.OvertimeRate);
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
    }

    private void NewPieceProductItemAdded(PieceProductAdded @event)
    {
        var pieceProduct = ProductItem.Create(@event.PieceworkProductId, @event.Quantity, @event.CurrentPrice);
        ProductItems.Add(pieceProduct);
    }
}