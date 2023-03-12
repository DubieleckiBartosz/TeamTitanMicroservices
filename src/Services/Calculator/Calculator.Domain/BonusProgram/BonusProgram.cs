using System.Net;
using Calculator.Domain.BonusProgram.Events;
using Calculator.Domain.BonusProgram.Generators;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;
using Shared.Domain.Tools;

namespace Calculator.Domain.BonusProgram;

public class BonusProgram : Aggregate
{
    public decimal BonusAmount { get; private set; }
    public string CreatedBy { get; private set; }
    public string CompanyCode { get; private set; }
    public DateTime? Expires { get; private set; }
    public string Reason { get; private set; }
    public List<Bonus>? Bonuses { get; private set; } 

    public BonusProgram()
    {
    }

    private BonusProgram(decimal bonusAmount, string createdBy, string companyCode, DateTime? expires, string reason)
    {
        var @event = NewBonusProgramCreated.Create(bonusAmount, createdBy, companyCode, expires,
            reason, Guid.NewGuid());

        Apply(@event);
        Enqueue(@event);
    }

    public static BonusProgram Create(decimal bonusAmount, string createdBy, string companyCode, DateTime? expires,
        string reason)
    {
        var newBonus = new BonusProgram(bonusAmount, createdBy, companyCode, expires, reason);

        return newBonus;
    }
      
    public void AddRecipientToBonus(string creator, string recipient, bool group)
    {
        var repeat = true;
        var bonusCode = string.Empty;
        while (repeat)
        {
            bonusCode = BonusCodeGenerator.GenerateBonusCode(Id.ToString(), group ? "DEP" : "ACC");
            var bonus = Bonuses?.FirstOrDefault(_ => _.BonusCode == bonusCode);

            if (bonus == null)
            {
                repeat = false;
            }
        } 

        var @event = BonusRecipientAdded.Create(creator, recipient, Id, bonusCode, group);
        Apply(@event);
        Enqueue(@event);
    }

    public void CancelBonusRecipient(string recipient, string bonusCode)
    {
        if (Bonuses == null)
        {
            throw new BusinessException("List is NULL", "List of recipients is NULL.",
                HttpStatusCode.NotFound);
        }

        var bonus = Bonuses.FirstOrDefault(_ => _.Recipient == recipient);
        if (bonus == null)
        {
            throw new BusinessException("Bonus recipient is NULL", "Recipient is not assigned to the program.",
                HttpStatusCode.NotFound);
        }

        var @event = BonusRecipientCanceled.Create(recipient, this.Id, bonusCode);

        Apply(@event);
        this.Enqueue(@event); 
    }
     
    protected override void When(IEvent @event)
    {
        switch (@event)
        {
            case BonusRecipientAdded e:
                this.RecipientToBonusProgramAdded(e); 
                break;
            case NewBonusProgramCreated e:
                this.BonusCreated(e);
                break;
            case BonusRecipientCanceled e:
                this.RecipientFromBonusProgramCanceled(e);
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
        Bonuses = new List<Bonus>();
    } 

    private void RecipientToBonusProgramAdded(BonusRecipientAdded @event)
    {
        if (Bonuses == null)
        {
            Bonuses = new List<Bonus>();
        }
        
        var newBonus = Bonus.Create(@event.Creator, @event.BonusCode, @event.Recipient, @event.GroupBonus);
        Bonuses!.Add(newBonus);
    }

    private void RecipientFromBonusProgramCanceled(BonusRecipientCanceled @event)
    {
        var bonus = Bonuses?.FirstOrDefault(_ => _.BonusCode == @event.BonusCode);

        if (Bonuses != null && bonus != null)
        {
            Bonuses.Replace(bonus, bonus.AsCanceled());
        } 
    } 
}