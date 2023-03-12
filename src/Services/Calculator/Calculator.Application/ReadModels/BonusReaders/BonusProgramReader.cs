using Calculator.Domain.BonusProgram.Events;
using Shared.Domain.Tools;
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
    public List<BonusReader>? Bonuses { get; private set; }

    public BonusProgramReader(Guid bonusProgramId, decimal bonusAmount, string createdBy, string companyCode, DateTime? expires, string reason)
    {
        this.Id = bonusProgramId;
        this.BonusAmount = bonusAmount;
        this.CreatedBy = createdBy;
        this.CompanyCode = companyCode;
        this.Expires = expires;
        this.Reason = reason;
        this.Bonuses = new List<BonusReader>();

    }
    public static BonusProgramReader BonusCreate(NewBonusProgramCreated @event)
    {
        return new BonusProgramReader(@event.BonusId, @event.BonusAmount, @event.CreatedBy, @event.CompanyCode,
            @event.Expires, @event.Reason);
    } 

    public BonusProgramReader RecipientToBonusProgramAdded(BonusRecipientAdded @event)
    {
        if (Bonuses == null)
        {
            Bonuses = new List<BonusReader>();
        }

        var newBonus = BonusReader.Create(@event.Creator, @event.BonusCode, @event.Recipient, @event.GroupBonus);
        Bonuses!.Add(newBonus);

        return this;
    }

    public BonusProgramReader RecipientFromBonusProgramCanceled(BonusRecipientCanceled @event)
    {
        var bonus = Bonuses?.FirstOrDefault(_ => _.BonusCode == @event.BonusCode);

        if (Bonuses != null && bonus != null)
        {
            Bonuses.Replace(bonus, bonus.AsCanceled());
        }

        return this;
    }
     

    public BonusReader GetBonusByCode(string code)
    {
        return Bonuses!.First(_ => _.BonusCode == code);
    }
    public BonusReader GetBonusByRecipient(string recipient)
    {
        return Bonuses!.First(_ => _.Recipient == recipient);
    } 
}