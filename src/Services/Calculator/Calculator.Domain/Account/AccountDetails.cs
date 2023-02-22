using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Domain.Account;

public class AccountDetails
{
    public string AccountOwnerExternalId { get; }
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
 
    private AccountDetails(string accountOwnerExternalId, string departmentCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
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
    /// <summary>
    /// Init
    /// </summary>
    /// <param name="accountOwnerExternalId"></param>
    /// <param name="departmentCode"></param>
    /// <param name="createdBy"></param>
    private AccountDetails(string accountOwnerExternalId, string departmentCode, string createdBy)
    {
        AccountOwnerExternalId = accountOwnerExternalId;
        DepartmentCode = departmentCode;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// To create an account state (snapshot)
    /// </summary>
    /// <param name="accountExternalId"></param>
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
    /// <returns></returns>
    public static AccountDetails CreateAccountDetails(string accountExternalId, string departmentCode,
        CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate)
    {
        return new AccountDetails(accountExternalId, departmentCode,
            countingType, accountStatus, activatedBy, createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate);
    }

    /// <summary>
    /// For loading data, e.g. from a snapshot
    /// </summary>
    /// <param name="accountExternalId"></param>
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
    /// <returns></returns>
    public static AccountDetails LoadAccountDetails(string accountExternalId, string departmentCode,
        CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate)
    {
        return new AccountDetails(accountExternalId, departmentCode,
            countingType, accountStatus, activatedBy, createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate);
    }

    public static AccountDetails Init(string accountOwnerExternalId, string departmentCode, string createdBy)
    {
        return new AccountDetails(accountOwnerExternalId, departmentCode, createdBy);
    }

    public void AssignData(CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate)
    {          
        //Validation
        CountingType = countingType;
        AccountStatus = accountStatus;
        ActivatedBy = activatedBy; 
        DeactivatedBy = deactivatedBy;
        IsActive = isActive;
        WorkDayHours = workDayHours;
        HourlyRate = hourlyRate;
        OvertimeRate = overtimeRate;
    }

    public void UpdateWorkDayHours(int newWorkDayHours)
    {
        //Validation
        WorkDayHours = newWorkDayHours;
    }
    public void UpdateOvertimeRate(decimal overtimeRate)
    {       
        //Validation
        OvertimeRate = overtimeRate;
    }

    public void UpdateHourlyRate(decimal hourlyRate)
    {
        //Validation
        HourlyRate = hourlyRate;
    }

    public void Deactivate(string deactivatedBy)
    {
        //Validation
        DeactivatedBy = deactivatedBy;
        AccountStatus = AccountStatus.Off;
        IsActive = false;
    }

    public void Activate(string activatedBy)
    {
        //Validation
        ActivatedBy = activatedBy;
        AccountStatus = AccountStatus.InUse;
        IsActive = true;
    } 
    
    public void UpdateCountingType(CountingType countingType)
    {
        //Validation
        CountingType = countingType; 
    } 
}