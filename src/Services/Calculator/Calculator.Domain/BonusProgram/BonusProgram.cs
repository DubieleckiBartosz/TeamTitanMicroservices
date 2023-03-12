using System.Net;
using Calculator.Domain.BonusProgram.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace Calculator.Domain.BonusProgram;

public class BonusProgram : Aggregate
{ 
    public decimal BonusAmount { get; private set; }
    public string CreatedBy { get; private set; }
    public string CompanyCode { get; private set; }
    public DateTime? Expires { get; private set; }
    public string Reason { get; private set; }
    public Dictionary<string, BonusCountRecipient>? Departments { get; private set; }
    public Dictionary<string, BonusCountRecipient>? Accounts { get; private set; }

    public BonusProgram()
    {
    }

    private BonusProgram(decimal bonusAmount, string createdBy, string companyCode, DateTime? expires, string reason)
    { 
        var @event = NewBonusProgramCreated.Create(bonusAmount, createdBy, companyCode, expires,
            reason, Guid.NewGuid());

        Apply(@event);
        this.Enqueue(@event);
    }

    public static BonusProgram Create(decimal bonusAmount, string createdBy, string companyCode, DateTime? expires, string reason)
    {
        var newBonus = new BonusProgram(bonusAmount, createdBy, companyCode, expires, reason);

        return newBonus;
    }

    public void AddDepartmentToBonus(string creator, string department)
    { 
        var @event = BonusDepartmentAdded.Create(creator, department, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void AddAccountToBonus(string creator, string account)
    { 
        var @event = BonusAccountAdded.Create(creator, account, this.Id);
        Apply(@event);
        this.Enqueue(@event);
    }

    public void RemoveDepartmentFromBonus(string department)
    {
        if (Departments == null)
        {
            throw new BusinessException("Dictionary is NULL", "Department dictionary is NULL.",
                HttpStatusCode.NotFound);
        }

        if (!Departments.TryGetValue(department, out var departmentValue))
        {
            throw new BusinessException("Department is NULL", "Department is not assigned to the program.",
                HttpStatusCode.NotFound);
        }

        var @event = DepartmentRemoved.Create(department, this.Id);

        Apply(@event);
        this.Enqueue(@event); 
    }

    public void RemoveAccountFromBonus(string account)
    {
        if (Accounts == null)
        {
            throw new BusinessException("Dictionary is NULL", "Account dictionary is NULL.", HttpStatusCode.NotFound);
        }

        if (!Accounts.TryGetValue(account, out var accountValue))
        {
            throw new BusinessException("Account is NULL", "Account is not assigned to the program.",
                HttpStatusCode.NotFound);
        }

        var @event = AccountRemoved.Create(account, this.Id);

        Apply(@event);
        this.Enqueue(@event); 
    }

    protected override void When(IEvent @event)
    {
        switch (@event)
        {
            case BonusAccountAdded e:
                this.AccountToBonusAdded(e);
                break;
            case BonusDepartmentAdded e:
                this.DepartmentToBonusAdded(e);
                break;
            case NewBonusProgramCreated e:
                this.BonusCreated(e);
                break;
            case DepartmentRemoved e:
                this.DepartmentFromBonusRemoved(e);
                break;
            case AccountRemoved e: 
                this.AccountFromBonusRemoved(e);
                break;
            default:
                break;
        }
    }

    public override Aggregate? FromSnapshot(ISnapshot snapshot)
    {
        return null;
    }

    private void BonusCreated(NewBonusProgramCreated @event)
    {
        Id = @event.BonusId;
        BonusAmount = @event.BonusAmount;
        CreatedBy = @event.CreatedBy;
        CompanyCode = @event.CompanyCode;
        Expires = @event.Expires;
        Reason = @event.Reason;
        Departments = new Dictionary<string, BonusCountRecipient>();
        Accounts = new Dictionary<string, BonusCountRecipient>();
    }
    private void DepartmentToBonusAdded(BonusDepartmentAdded @event)
    {
        if (Departments == null)
        {
            Departments = new Dictionary<string, BonusCountRecipient>();
        }

        if (Departments.TryGetValue(@event.Department, out var department))
        {
            department.AddNewBonus(@event.Creator);
            Departments[@event.Department] = department;
        }
        else
        {
            var newDepartmentBonus = BonusCountRecipient.Create();
            newDepartmentBonus.AddNewBonus(@event.Creator);

            Departments.Add(@event.Department, newDepartmentBonus);
        }
    }

    private void AccountToBonusAdded(BonusAccountAdded @event)
    {
        if (Accounts == null)
        {
            Accounts = new Dictionary<string, BonusCountRecipient>();
        }

        if (Accounts.TryGetValue(@event.Account, out var account))
        {
            account.AddNewBonus(@event.Creator);
            Accounts[@event.Account] = account;
        }
        else
        {
            var newAccountBonus = BonusCountRecipient.Create();
            newAccountBonus.AddNewBonus(@event.Creator);

            Accounts.Add(@event.Account, newAccountBonus);
        }
    }

    private void DepartmentFromBonusRemoved(DepartmentRemoved @event)
    {
        if (Departments != null && Departments.TryGetValue(@event.Department, out var departmentBonus))
        {
            departmentBonus.CancelBonus();
            if (departmentBonus.Count == 0)
            {
                Departments.Remove(@event.Department);
            }
            else
            {
                Departments[@event.Department] = departmentBonus;
            }
        } 
    }

    private void AccountFromBonusRemoved(AccountRemoved @event)
    {  
        if (Accounts != null && Accounts.TryGetValue(@event.Account, out var accountBonus))
        {
            accountBonus.CancelBonus();
            if (accountBonus.Count == 0)
            {
                Accounts.Remove(@event.Account);
            }
            else
            {
                Accounts[@event.Account] = accountBonus;
            }
        } 
    }

}