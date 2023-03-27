using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class EmployeeContract : Entity
{
    public string Position { get; }
    public string ContractNumber { get; }
    public ContractType ContractType { get; }
    public SettlementType? SettlementType { get; }
    public decimal Salary { get; }
    public decimal? HourlyRate { get; }
    public decimal? OvertimeRate { get; }
    public TimeRange TimeRange { get; }
    public int NumberHoursPerDay { get; }
    public int FreeDaysPerYear { get; }
    public string BankAccountNumber { get; } 
    public bool PaidIntoAccount { get; } 
    public int PaymentMonthDay { get; set; }
    public string CreatedBy { get; }

    private EmployeeContract(string position,
        ContractType contractType, SettlementType? settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string bankAccountNumber, bool paidIntoAccount, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    { 
        Position = position;
        ContractNumber = Guid.NewGuid().ToString();
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

    public static EmployeeContract Create(string position,
        ContractType contractType, SettlementType? settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string bankAccountNumber, bool paidIntoAccount, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    {
        return new EmployeeContract(position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paidIntoAccount, createdBy, hourlyRate,
            overtimeRate, paymentMonthDay);
    }
}