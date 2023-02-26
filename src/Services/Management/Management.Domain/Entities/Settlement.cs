using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Settlement : Entity
{ 
    public bool IsPaid { get; private set; }
    public int EmployeeId { get; } 
    public SettlementMoney Value { get; }
    public Period Period { get; }

    private Settlement(bool isPaid, int employeeId, SettlementMoney value, Period period)
    {
        IsPaid = isPaid;
        EmployeeId = employeeId;
        Value = value;
        Period = period;
    }

    public static Settlement Create(bool isPaid, int employeeId, SettlementMoney value, Period period)
    {
        return new Settlement(isPaid, employeeId, value, period);
    }

    public void AsPaid()
    {
        this.IsPaid = true;
    }
     
}