using Management.Domain.Events;
using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace Management.Domain.Entities;

public class Contract : Entity
{
    public string Position { get; } 
    public ContractType ContractType { get; }
    public SettlementType SettlementType { get; private set; }
    public decimal? Salary { get; private set; }
    public decimal? HourlyRate { get; private set; }
    public decimal? OvertimeRate { get; private set; }
    public TimeRange TimeRange { get; }
    public int NumberHoursPerDay { get; private set; }
    public int FreeDaysPerYear { get; }
    public string? BankAccountNumber { get; private set; }  
    public int PaymentMonthDay { get; private set; }
    public string CreatedBy { get; }

    private Contract(string position,
        ContractType contractType, SettlementType settlementType, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, string createdBy, int paymentMonthDay)
    {
        Position = position;
        ContractType = contractType;
        SettlementType = settlementType;
        TimeRange = timeRange;
        NumberHoursPerDay = numberHoursPerDay;
        FreeDaysPerYear = freeDaysPerYear;
        BankAccountNumber = bankAccountNumber;
        CreatedBy = createdBy;
        PaymentMonthDay = paymentMonthDay;
    }

    public Contract(int id, int version, string position,
        ContractType contractType, SettlementType settlementType, decimal? salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, string createdBy, decimal? hourlyRate,
        decimal? overtimeRate, int paymentMonthDay) : this(position,
        contractType, settlementType, timeRange,
        numberHoursPerDay, freeDaysPerYear, bankAccountNumber, createdBy, paymentMonthDay)
    {
        Id = id;
        Version = version;
        Salary = salary;
        OvertimeRate = overtimeRate;
        HourlyRate = hourlyRate;
    }

    public static Contract Create(string position,
        ContractType contractType, SettlementType settlementType, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, 
        string createdBy, int paymentMonthDay)
    {
        return new Contract(position, contractType, settlementType, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, createdBy,   paymentMonthDay);
    }

    public static Contract Load(int id, int version, string position,
        ContractType contractType, SettlementType settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    {
        return new Contract(id, version, position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, createdBy, hourlyRate,
            overtimeRate, paymentMonthDay);
    } 

    public void UpdateSalary(decimal salary, Guid account)
    {
        CheckPossibilityOfChangeOrThrow();
        Salary = salary; 
        Events.Add(new SalaryChanged(account, salary));
    }

    public void UpdateFinancialData(decimal? salary, decimal? hourlyRate, decimal? overtimeRate, Guid account)
    {
        CheckPossibilityOfChangeOrThrow();

        if (Salary != null)
        {
            Salary = salary;
        }

        if (hourlyRate.HasValue || overtimeRate.HasValue)
        {
            CheckSettlementTypeOrThrow();
            (HourlyRate, OvertimeRate) = (hourlyRate, overtimeRate ?? hourlyRate);
        }

        Events.Add(new FinancialDataChanged(account, salary, hourlyRate, overtimeRate));
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
    
    public void UpdateDayHours(int newDayHours, Guid account)
    {
        CheckPossibilityOfChangeOrThrow();
        NumberHoursPerDay = newDayHours; 
        Events.Add(new DayHoursChanged(account, newDayHours)); 
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