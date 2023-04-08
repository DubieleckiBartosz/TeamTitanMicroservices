using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace Management.Domain.Entities;

public class EmployeeContract : Entity
{
    public string Position { get; } 
    public ContractType ContractType { get; }
    public SettlementType SettlementType { get; private set; }
    public decimal Salary { get; private set; }
    public decimal? HourlyRate { get; private set; }
    public decimal? OvertimeRate { get; private set; }
    public TimeRange TimeRange { get; }
    public int NumberHoursPerDay { get; private set; }
    public int FreeDaysPerYear { get; }
    public string? BankAccountNumber { get; private set; } 
    public bool PaidIntoAccount { get; private set; }
    public int PaymentMonthDay { get; private set; }
    public string CreatedBy { get; } 
    private EmployeeContract(string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, bool paidIntoAccount, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    { 
        Position = position; 
        ContractType = contractType;
        SettlementType = settlementType;
        Salary = salary;
        TimeRange = timeRange;
        NumberHoursPerDay = numberHoursPerDay;
        FreeDaysPerYear = freeDaysPerYear;
        BankAccountNumber = bankAccountNumber;
        PaidIntoAccount = paidIntoAccount;
        CreatedBy = createdBy;
        HourlyRate = hourlyRate;
        OvertimeRate = overtimeRate;
        PaymentMonthDay = paymentMonthDay;
    }

    public EmployeeContract(int id, int version, string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, bool paidIntoAccount, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay) : this(position,
        contractType, settlementType, salary, timeRange,
        numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paidIntoAccount, createdBy,
        hourlyRate, overtimeRate, paymentMonthDay)
    {
        Id = id;
        Version = version;
    }

    public static EmployeeContract Create(string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, bool paidIntoAccount, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    {
        return new EmployeeContract(position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paidIntoAccount, createdBy, hourlyRate,
            overtimeRate, paymentMonthDay);
    }

    public static EmployeeContract Load(int id, int version, string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, bool paidIntoAccount, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    {
        return new EmployeeContract(id, version, position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paidIntoAccount, createdBy, hourlyRate,
            overtimeRate, paymentMonthDay);
    }

    public void UpdateSalary(decimal salary)
    {
        CheckPossibilityOfChangeOrThrow();
        Salary = salary; 
        IncrementVersion();
    }

    public void UpdateFinancialData(decimal? hourlyRate, decimal? overtimeRate)
    {
        CheckPossibilityOfChangeOrThrow();
        CheckSettlementTypeOrThrow();
        (HourlyRate, OvertimeRate) = (hourlyRate, overtimeRate);
        IncrementVersion();
    }
     
    public void UpdateSettlementType(int settlementType)
    {
        CheckPossibilityOfChangeOrThrow();
        var newSettlementType = Enumeration.GetById<SettlementType>(settlementType);
        SettlementType = newSettlementType;
        IncrementVersion();
    }

    public void AddOrUpdateBankAccountNumber(string bankAccount)
    {
        CheckPossibilityOfChangeOrThrow();
        BankAccountNumber = bankAccount;
        if (PaidIntoAccount == false)
        {
            PaidIntoAccount = true;
        }

        IncrementVersion();
    }

    public void UpdatePaymentMonthDay(int paymentMonthDay)
    {
        CheckPossibilityOfChangeOrThrow();
        PaymentMonthDay = paymentMonthDay;
        IncrementVersion();
    }

    private void CheckSettlementTypeOrThrow()
    {
        if (!IsValidSettlementType)
        {
            throw new BusinessException("Invalid settlement type",
                "This operation cannot be performed if the salary is fixed");
        }  
    }

    private void CheckPossibilityOfChangeOrThrow()
    {
        if (!IsModifiable)
        {
            throw new BusinessException("Invalid contract end date",
                "You cannot change a contract that has already expired");
        }
    }

    private bool IsValidSettlementType => SettlementType != SettlementType.FixedSalary;
    private bool IsModifiable => TimeRange.EndContract == null || TimeRange.EndContract > DateTime.Today;
}