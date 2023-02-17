using Management.Domain.Types;
using Management.Domain.ValueObjects;

namespace Management.Domain.Entities;

public class EmployeeContract
{
    public int DepartmentId { get; }
    public Employee Employee { get; }
    public string Position { get; private set; }
    public string ContractNumber { get; }
    public ContractType ContractType { get; }
    public SettlementType? SettlementType { get; }
    public decimal Salary { get; set; }
    public TimeRange TimeRange { get; set; }
    public int NumberHoursPerDay { get; set; }
    public int FreeDaysPerYear { get; private set; }
    public string BankAccountNumber { get; private set; }
    public bool PaidIntoAccount { get; private set; }

}