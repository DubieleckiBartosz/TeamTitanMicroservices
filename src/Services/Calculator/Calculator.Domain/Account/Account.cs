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
    
    private Account(Guid accountId, string accountOwnerExternalId, string departmentCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string? createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate)
    {
        Id = accountId;
        Details = AccountDetails.CreateAccountDetails(
            accountOwnerExternalId, departmentCode,
            countingType, accountStatus, activatedBy, 
            createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate);

        Watcher = Watcher.Create();
        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }

    public void ActiveAccount()
    {
    }

    public void DeactivateAccount()
    {
    }

    public void UpdateWorkDayHours()
    {
    }

    public void AddNewPayment()
    {
    }

    public void Calculate()
    {
    }

    public void AddNewWorkDay(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy)
    {
        var workDay = WorkDay.Create(date, hoursWorked, overtime, isDayOff, createdBy);
    }

    public AccountSnapshot CreateSnapshot()
    {
        return AccountSnapshot.Create(this.Id).Set(this);
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
        throw new NotImplementedException();
    }
}