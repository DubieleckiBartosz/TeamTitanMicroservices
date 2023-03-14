using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Domain.Account;

public class AccountState
{
    public AccountDetails Details { get; } //Setter for serializer
    public List<ProductItem> ProductItems { get; } //Setter for serializer
    public List<WorkDay> WorkDays { get; } //Setter for serializer

    //Constructor for serializer
    public AccountState()
    {
    }

    private AccountState(string accountOwnerExternalId, string departmentCode, CountingType countingType,
        AccountStatus accountStatus, string? activatedBy, string createdBy, string? deactivatedBy, bool isActive,
        int workDayHours, decimal? hourlyRate, decimal? overtimeRate, decimal balance)
    {
        Details = AccountDetails.CreateAccountDetails(accountOwnerExternalId, departmentCode,
            countingType, accountStatus, activatedBy, createdBy, deactivatedBy, isActive,
            workDayHours, hourlyRate, overtimeRate, balance);

        ProductItems = new List<ProductItem>();
        WorkDays = new List<WorkDay>();
    }

    public static AccountState CreateAccountState(Account account)
    {
        var details = account.Details!;
        return new AccountState(details.AccountOwner, details.DepartmentCode,
            details.CountingType, details.AccountStatus, details.ActivatedBy, details.CreatedBy, details.DeactivatedBy,
            details.IsActive,
            details.WorkDayHours, details.HourlyRate, details.OvertimeRate, details.Balance);
    }
}