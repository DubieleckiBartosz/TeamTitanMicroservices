using Calculator.Domain.Types;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.Entities;

public class PersonToAccountedFor : Aggregate
{
    public string PersonToAccountedExternalId { get; }
    public string DepartmentCode { get; }
    public CountingType CountingType { get; }
    public bool IsActive { get; private set; }
    public List<Payment> Payments { get; private set; }
    public List<WorkDay> WorkDays { get; private set; } 
    public PersonToAccountedFor(string personToAccountedExternalId, bool isActive, string departmentCode)
    {
        PersonToAccountedExternalId = personToAccountedExternalId;
        IsActive = isActive;
        DepartmentCode = departmentCode;
        Payments = new List<Payment>();
        WorkDays = new List<WorkDay>();
    }

    protected override void When(IEvent @event)
    {
        throw new NotImplementedException();
    }
}