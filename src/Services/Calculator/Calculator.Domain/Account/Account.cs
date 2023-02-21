using Calculator.Domain.Account.Events;
using Calculator.Domain.Account.Snapshots;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.Account;

public class Account : Aggregate
{
    public AccountDetails Details { get; }
    public List<ProductItem> ProductItems { get; private set; }
    public List<WorkDay> WorkDays { get; private set; }

    private Account(string accountOwnerExternalId, string departmentCode, CountingType countingType,
        string creator, int workDayHours, decimal? overtimeRate, decimal? hourlyRate)
    {
        Details = AccountDetails.CreateAccountDetails(
            accountOwnerExternalId, 
            departmentCode, countingType,
            AccountStatus.New, null,
            creator, null, false,
            workDayHours, hourlyRate, overtimeRate);

        Watcher = Watcher.Create();
        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }
    
    //Load
    private Account(Guid accountId, int version, string accountOwnerExternalId, string departmentCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate)
    {
        Id = accountId;
        Version = version;
        Details = AccountDetails.CreateAccountDetails(
            accountOwnerExternalId, departmentCode,
            countingType, accountStatus, activatedBy, 
            createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate);

        Watcher = Watcher.Create();
        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }
    
    public void ActiveAccount(string activatedBy)
    {
    }

    public void DeactivateAccount(string deactivatedBy)
    {
    }

    public void UpdateWorkDayHours(int newWorkDayHours)
    {
    }

    public void UpdateHourlyRate(decimal newHourlyRate)
    {
    }
    
    public void UpdateOvertimeRate(decimal newOvertimeRate)
    {
    }
    
    public void UpdateCountingType(CountingType newCountingType)
    {
    }

    public void AddNewWorkDay(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy)
    {
        var workDay = WorkDay.Create(date, hoursWorked, overtime, isDayOff, createdBy);
    }

    public void AddNewPieceProductItem(Guid pieceworkProductId, decimal quantity, decimal currentPrice)
    { 
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
            case AccountStatusChanged e:
                break;
            case HourlyRateChanged e:
                break;
            case NewWorkDayAdded e:
                break;
            case OvertimeRateChanged e:
                break;
            case PieceProductAdded e:
                break;
            case WorkDayHoursChanged e:
                break;
            default:
                break;
        }
    }


}