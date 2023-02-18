using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class EmployeeContract : Entity
{
    public int DepartmentId { get; }
    public Employee Employee { get; }
    public string Position { get; }
    public string ContractNumber { get; }
    public ContractType ContractType { get; }
    public SettlementType? SettlementType { get; }
    public decimal Salary { get; private set; }
    public TimeRange TimeRange { get; }
    public int NumberHoursPerDay { get; private set; }
    public int FreeDaysPerYear { get; private set; }
    public string BankAccountNumber { get; private set; }
    public bool PaidIntoAccount { get; private set; }
    public string CreatedBy { get; } 

    private EmployeeContract(int departmentId, Employee employee, string position, 
        ContractType contractType, SettlementType? settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string bankAccountNumber, bool paidIntoAccount, string createdBy)
    {
        DepartmentId = departmentId;
        Employee = employee;
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
    }

    public static EmployeeContract Create(int departmentId, Employee employee, string position, 
        ContractType contractType, SettlementType? settlementType, decimal salary, TimeRange timeRange,
        int numberHoursPerDay, int freeDaysPerYear, string bankAccountNumber, bool paidIntoAccount, string createdBy)
    {
        return new EmployeeContract(departmentId, employee, position, contractType, settlementType, salary, timeRange,
            numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paidIntoAccount, createdBy);
    }
}