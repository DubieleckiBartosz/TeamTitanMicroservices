using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Domain.Account;

public class AccountDetails
{
    public string AccountOwnerExternalId { get; }
    public string DepartmentCode { get; }
    public CountingType CountingType { get; }
    public AccountStatus AccountStatus { get; }
    public string? ActivatedBy { get; }
    public string? CreatedBy { get; }
    public string? DeactivatedBy { get; }
    public bool IsActive { get; }
    public int WorkDayHours { get; }
    public decimal? HourlyRate { get; }
    public decimal? OvertimeRate { get; }
    public AccountDetails(string accountOwnerExternalId, string departmentCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string? createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate)
    {
        AccountOwnerExternalId = accountOwnerExternalId;
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
    }

    public static AccountDetails CreateAccountDetails(string accountExternalId, string departmentCode,
        CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string? createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate)
    {
        return new AccountDetails(accountExternalId, departmentCode,
            countingType, accountStatus, activatedBy, createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate);
    }
}