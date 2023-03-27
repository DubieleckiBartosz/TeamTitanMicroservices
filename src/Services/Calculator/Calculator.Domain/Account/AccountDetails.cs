using Calculator.Domain.Account.Events;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account;

public class AccountDetails
{ 
    public decimal Balance { get; private set; } 
    public string AccountOwner { get; private set; } //Setter for serializer
    public string CompanyCode { get; private set; } //Setter for serializer
    public CountingType CountingType { get; private set; }
    public AccountStatus AccountStatus { get; private set; }
    public string? ActivatedBy { get; private set; }
    public string CreatedBy { get; private set; } //Setter for serializer
    public string? DeactivatedBy { get; private set; }
    public bool IsActive { get; private set; }
    public int WorkDayHours { get; private set; }
    public decimal? HourlyRate { get; private set; }
    public decimal? OvertimeRate { get; private set; } 
    public DateTime? ExpirationDate { get; private set; }
    public int? SettlementDayMonth { get; private set; }


    //Constructor for serializer
    public AccountDetails()
    {
    }

    /// <summary>
    /// For logic
    /// </summary>
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
    private AccountDetails(string accountOwner, string companyCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance, DateTime? expirationDate, int? settlementDayMonth)
    {
        AccountOwner = accountOwner;
        CompanyCode = companyCode;
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
        ExpirationDate = expirationDate;
        SettlementDayMonth = settlementDayMonth;
    }

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="accountOwner"></param>
    /// <param name="companyCode"></param>
    /// <param name="createdBy"></param>
    private AccountDetails(string accountOwner, string companyCode, string createdBy)
    {
        Balance = 0;
        AccountOwner = accountOwner;
        CompanyCode = companyCode;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// To create an account state (snapshot)
    /// </summary>
    /// <param name="accountExternalId"></param>
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
    /// <returns></returns>
    public static AccountDetails CreateAccountDetails(string accountExternalId, string companyCode,
        CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance, DateTime? expirationDate, int? settlementDayMonth)
    {
        return new AccountDetails(accountExternalId, companyCode,
            countingType, accountStatus, activatedBy, createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate, balance, expirationDate, settlementDayMonth: settlementDayMonth);
    }

    /// <summary>
    /// For loading data, e.g. from a snapshot
    /// </summary>
    /// <param name="accountExternalId"></param>
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
    /// <returns></returns>
    public static AccountDetails LoadAccountDetails(string accountExternalId, string companyCode,
        CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance, DateTime? expirationDate, int? settlementDayMonth)
    {
        return new AccountDetails(accountExternalId, companyCode,
            countingType, accountStatus, activatedBy, createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate, balance, expirationDate, settlementDayMonth);
    }

    public static AccountDetails Init(string accountOwner, string companyCode, string createdBy)
    {
        return new AccountDetails(accountOwner, companyCode, createdBy);
    }

    public void AssignData(CountingType countingType,
        AccountStatus accountStatus, bool isActive,
        int workDayHours, int settlementDayMonth, DateTime? expirationDate)
    {           
        CountingType = countingType;
        AccountStatus = accountStatus;
        IsActive = isActive;
        WorkDayHours = workDayHours; 
        ExpirationDate = expirationDate;
        SettlementDayMonth = settlementDayMonth;
    }

    public void AssignFinancialData(decimal? hourlyRate, decimal? overtimeRate)
    { 
        HourlyRate = hourlyRate ?? HourlyRate;
        OvertimeRate = overtimeRate ?? OvertimeRate; 
    }


    public void UpdateWorkDayHours(int newWorkDayHours)
    { 
        WorkDayHours = newWorkDayHours;
    }
    public void UpdateOvertimeRate(decimal overtimeRate)
    {        
        OvertimeRate = overtimeRate;
    }

    public void UpdateHourlyRate(decimal hourlyRate)
    { 
        HourlyRate = hourlyRate;
    }

    public void Deactivate(string deactivatedBy)
    { 
        DeactivatedBy = deactivatedBy;
        AccountStatus = AccountStatus.Off;
        IsActive = false;
    }

    public void Activate(string activatedBy)
    { 
        ActivatedBy = activatedBy;
        AccountStatus = AccountStatus.InUse;
        IsActive = true;
    } 
    
    public void UpdateCountingType(CountingType countingType)
    {
        //Validation
        CountingType = countingType; 
    } 
    
    public void IncreaseBalance(IEvent @event)
    {
        var value = 0.0m;
         
        if (CountingType == CountingType.Piecework)
        {
            if (@event is PieceProductAdded productItem)
            {
                value = productItem.Quantity * productItem.CurrentPrice;
            }
        }

        if (CountingType == CountingType.ForAnHour)
        {
            if (@event is WorkDayAdded workDay)
            {
                value = workDay.HoursWorked * HourlyRate!.Value + workDay.Overtime * (HourlyRate ?? 0);
            } 
        }

        if (@event is BonusAdded bonus)
        {
            value = bonus.BonusAmount;
        }
         
        Balance += value; 
    }

    public void DecreaseBalance(decimal valueDecrease)
    {
        Balance -= valueDecrease;
    }

    public void ClearBalance()
    { 
        Balance = 0; 
    } 
}