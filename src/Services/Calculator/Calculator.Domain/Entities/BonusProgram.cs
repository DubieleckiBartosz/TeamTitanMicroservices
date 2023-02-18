using Calculator.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.Entities;

public class BonusProgram : Aggregate  
{
    public decimal BonusAmount { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Expires { get; set; }
    public Dictionary<string, BonusCountRecipient>? DepartmentCodes { get; }
    public Dictionary<string, BonusCountRecipient>? Persons { get; }

    public void AddDepartmentToBonus()
    {

    }

    public void AddPersonToBonus()
    {

    }

    public void RemoveDepartmentFromBonus()
    {

    }

    public void RemovePersonFromBonus()
    {

    }

    protected override void When(IEvent @event)
    {
        throw new NotImplementedException();
    }

}