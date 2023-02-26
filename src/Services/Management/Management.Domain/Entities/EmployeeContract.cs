using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class EmployeeContract : Entity
{
    public int DepartmentId { get; } 
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
    public string CreatedBy { get; }

    private EmployeeContract(int departmentId, string position,
        ContractType contractType, SettlementType? settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string bankAccountNumber, bool paidIntoAccount, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate)
    {
        DepartmentId = departmentId;
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
    }

    public static EmployeeContract Create(int departmentId, string position,
        ContractType contractType, SettlementType? settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string bankAccountNumber, bool paidIntoAccount, string createdBy,
        decimal? hourlyRate, decimal? overtimeRate)
    {
        return new EmployeeContract(departmentId, position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paidIntoAccount, createdBy, hourlyRate,
            overtimeRate);
    }

}