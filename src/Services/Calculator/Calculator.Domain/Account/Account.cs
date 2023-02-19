using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.Account;

public class Account : Aggregate
{
    public string AccountExternalId { get; }
    public string DepartmentCode { get; }
    public CountingType CountingType { get; private set; }
    public AccountStatus AccountStatus { get; private set; }
    public string? ActivatedBy { get; private set; }
    public string? CreatedBy { get; }
    public string? DeactivatedBy { get; private set; }
    public bool IsActive { get; private set; }
    public int WorkDayHours { get; private set; }
    public decimal? HourlyRate { get; }
    public decimal? OvertimeRate { get; }
    public List<ProductItem> ProductItems { get; }
    public List<WorkDay> WorkDays { get; }

    public Account(string accountExternalId, string departmentCode, CountingType countingType,
        string creator, int workDayHours, decimal? overtimeRate, decimal? hourlyRate)
    {
        AccountExternalId = accountExternalId;
        DepartmentCode = departmentCode;
        CountingType = countingType;
        AccountStatus = AccountStatus.New;
        IsActive = false;
        CreatedBy = creator;
        WorkDayHours = workDayHours;
        OvertimeRate = overtimeRate;
        HourlyRate = hourlyRate; 
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

    public void AddNewWorkDay(DateTime date, int hoursWorked, int overtime, bool isDayOff, string createdBy)
    {
        var workDay = WorkDay.Create(date, hoursWorked, overtime, isDayOff, createdBy);
    }

    protected override void When(IEvent @event)
    {
        throw new NotImplementedException();
    }
}