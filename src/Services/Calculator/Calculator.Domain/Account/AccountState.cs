using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Domain.Account;

public class AccountState
{
    public AccountDetails Details { get; private set; } //Setter for serializer
    public List<ProductItem> ProductItems { get; private set; } //Setter for serializer
    public List<WorkDay> WorkDays { get; private set; } //Setter for serializer
    public List<Settlement> Settlements { get; private set; } = new List<Settlement>();
    public List<Bonus> Bonuses { get; private set; } = new List<Bonus>();

    //Constructor for serializer
    public AccountState()
    {
    }

    private AccountState(string accountOwnerExternalId, string companyCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance,
        DateTime? expiration, int settlementDayMonth, List<Settlement> settlements)
    {
        Details = AccountDetails.CreateAccountDetails(accountOwnerExternalId, companyCode,
            countingType, accountStatus, activatedBy, createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate, balance, expiration, settlementDayMonth);

        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
        Bonuses = new List<Bonus>();
        Settlements = settlements;
    }

    public static AccountState CreateAccountState(Account account)
    {
        var details = account.Details!;
        return new AccountState(
            details.AccountOwner, 
            details.CompanyCode,
            details.CountingType, 
            details.AccountStatus,
            details.ActivatedBy, 
            details.CreatedBy, 
            details.DeactivatedBy,
            details.IsActive,
            details.WorkDayHours, 
            details.HourlyRate, 
            details.OvertimeRate, 
            details.Balance, 
            details.ExpirationDate,
            details.SettlementDayMonth!.Value,
            account.Settlements);
    }
}