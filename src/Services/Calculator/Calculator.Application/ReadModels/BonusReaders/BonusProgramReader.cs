using Calculator.Domain.BonusProgram.Events;
using Shared.Implementations.Projection;

namespace Calculator.Application.ReadModels.BonusReaders;

public class BonusProgramReader : IRead
{
    public Guid Id { get; }
    public decimal BonusAmount { get; private set; }
    public string CreatedBy { get; private set; }
    public string CompanyCode { get; private set; }
    public DateTime? Expires { get; private set; }
    public string Reason { get; private set; }
    public Dictionary<string, BonusRecipientReader>? Departments { get; private set; }
    public Dictionary<string, BonusRecipientReader>? Accounts { get; private set; }

    public BonusProgramReader(Guid bonusProgramId, decimal bonusAmount, string createdBy, string companyCode, DateTime? expires, string reason)
    {
        this.Id = bonusProgramId;
        this.BonusAmount = bonusAmount;
        this.CreatedBy = createdBy;
        this.CompanyCode = companyCode;
        this.Expires = expires;
        this.Reason = reason;
        Departments = new Dictionary<string, BonusRecipientReader>();
        Accounts = new Dictionary<string, BonusRecipientReader>();
    }
    public static BonusProgramReader BonusCreate(NewBonusProgramCreated @event)
    {
        return new BonusProgramReader(@event.BonusId, @event.BonusAmount, @event.CreatedBy, @event.CompanyCode,
            @event.Expires, @event.Reason);
    }
    public BonusProgramReader DepartmentToBonusAdded(BonusDepartmentAdded @event)
    {
        if (Departments == null)
        {
            Departments = new Dictionary<string, BonusRecipientReader>();
        }

        if (Departments.TryGetValue(@event.Department, out var department))
        {
            department.AddNewBonus(@event.Creator);
            Departments[@event.Department] = department;
        }
        else
        {
            var newDepartmentBonus = BonusRecipientReader.Create();
            newDepartmentBonus.AddNewBonus(@event.Creator);

            Departments.Add(@event.Department, newDepartmentBonus);
        }

        return this;
    }

    public BonusProgramReader AccountToBonusAdded(BonusAccountAdded @event)
    {
        if (Accounts == null)
        {
            Accounts = new Dictionary<string, BonusRecipientReader>();
        }

        if (Accounts.TryGetValue(@event.Account, out var account))
        {
            account.AddNewBonus(@event.Creator);
            Accounts[@event.Account] = account;
        }
        else
        {
            var newAccountBonus = BonusRecipientReader.Create();
            newAccountBonus.AddNewBonus(@event.Creator);

            Accounts.Add(@event.Account, newAccountBonus);
        }

        return this;
    }

    public BonusProgramReader DepartmentFromBonusRemoved(DepartmentRemoved @event)
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

        return this;
    }

    public BonusProgramReader AccountFromBonusRemoved(AccountRemoved @event)
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

        return this;
    }

    public BonusRecipientReader GetDepartmentByCode(string code)
    {
        return Departments![code];
    }

    public BonusRecipientReader GetAccountByCode(string code)
    {
        return Accounts![code];
    }
}