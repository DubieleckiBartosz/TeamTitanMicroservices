using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Calculator.Domain.BonusProgram;

public class BonusProgram : Aggregate
{
    public decimal BonusAmount { get; private set; }
    public string CreatedBy { get; }
    public string CompanyCode { get;}
    public DateTime? Expires { get; private set; }
    public string Reason { get;}
    public Dictionary<string, BonusCountRecipient>? Departments { get; private set; }
    public Dictionary<string, BonusCountRecipient>? Persons { get; private set; }

    public BonusProgram(decimal bonusAmount, string createdBy, string companyCode, DateTime? expires, string reason)
    {
        BonusAmount = bonusAmount;
        CreatedBy = createdBy;
        CompanyCode = companyCode;
        Expires = expires;
        Reason = reason;
        Departments = new Dictionary<string, BonusCountRecipient>();
        Persons = new Dictionary<string, BonusCountRecipient>();
    }
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