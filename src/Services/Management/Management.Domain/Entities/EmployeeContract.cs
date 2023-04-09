using Management.Domain.Events;
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
    public int PaymentMonthDay { get; private set; }
    public string CreatedBy { get; } 
    private EmployeeContract(string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, string createdBy,
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
        CreatedBy = createdBy;
        HourlyRate = hourlyRate;
        OvertimeRate = overtimeRate;
        PaymentMonthDay = paymentMonthDay;
    }

    public EmployeeContract(int id, int version, string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay) : this(position,
        contractType, settlementType, salary, timeRange,
        numberHoursPerDay, freeDaysPerYear, bankAccountNumber, createdBy,
        hourlyRate, overtimeRate, paymentMonthDay)
    {
        Id = id;
        Version = version;
    }

    public static EmployeeContract Create(string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    {
        return new EmployeeContract(position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, createdBy, hourlyRate,
            overtimeRate, paymentMonthDay);
    }

    public static EmployeeContract Load(int id, int version, string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    {
        return new EmployeeContract(id, version, position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, createdBy, hourlyRate,
            overtimeRate, paymentMonthDay);
    }

    public void UpdateSalary(decimal salary, Guid account)
    {
        CheckPossibilityOfChangeOrThrow();
        Salary = salary; 
        Events.Add(new SalaryChanged(account, salary));
    }

    public void UpdateHourlyRates(decimal? hourlyRate, decimal? overtimeRate, Guid account)
    {
        CheckPossibilityOfChangeOrThrow();
        CheckSettlementTypeOrThrow();
        (HourlyRate, OvertimeRate) = (hourlyRate, overtimeRate ?? hourlyRate);
        Events.Add(new HourlyRatesChanged(account, hourlyRate, overtimeRate));
    }

    public void UpdateSettlementType(int settlementType, Guid account)
    {
        CheckPossibilityOfChangeOrThrow();
        var newSettlementType = Enumeration.GetById<SettlementType>(settlementType);
        SettlementType = newSettlementType;
        Events.Add(new SettlementTypeChanged(account, newSettlementType.Id)); 
    }

    public void AddOrUpdateBankAccountNumber(string bankAccount)
    {
        CheckPossibilityOfChangeOrThrow();
        BankAccountNumber = bankAccount; 
        IncrementVersion();
    }

    public void UpdatePaymentMonthDay(int paymentMonthDay, Guid account)
    {
        CheckPossibilityOfChangeOrThrow();
        PaymentMonthDay = paymentMonthDay;
        Events.Add(new PaymentMonthDayChanged(account, paymentMonthDay));

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