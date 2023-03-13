using Calculator.Domain.BonusProgram.Events;
using Shared.Domain.Tools;
using Shared.Implementations.Projection;

namespace Calculator.Application.ReadModels.BonusReaders;

public class BonusProgramReader : IRead
{
    public Guid Id { get; }
    public decimal BonusAmount { get; }
    public string CreatedBy { get; }
    public string CompanyCode { get; }
    public DateTime? Expires { get; }
    public string Reason { get; }
    public List<BonusReader>? Bonuses { get; private set; }
    
    /// <summary>
    /// For logic
    /// </summary>
    /// <param name="bonusProgramId"></param>
    /// <param name="bonusAmount"></param>
    /// <param name="createdBy"></param>
    /// <param name="companyCode"></param>
    /// <param name="expires"></param>
    /// <param name="reason"></param>
    private BonusProgramReader(Guid bonusProgramId, decimal bonusAmount, string createdBy, string companyCode,
        DateTime? expires, string reason)
    {
        Id = bonusProgramId;
        BonusAmount = bonusAmount;
        CreatedBy = createdBy;
        CompanyCode = companyCode;
        Expires = expires;
        Reason = reason;
        Bonuses = new List<BonusReader>();
    }

    /// <summary>
    /// For load
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bonusAmount"></param>
    /// <param name="createdBy"></param>
    /// <param name="companyCode"></param>
    /// <param name="expires"></param>
    /// <param name="reason"></param>
    /// <param name="bonuses"></param>
    private BonusProgramReader(Guid id, decimal bonusAmount, string createdBy, string companyCode, DateTime? expires,
        string reason, List<BonusReader>? bonuses) : this(id, bonusAmount, createdBy, companyCode, expires, reason)
    {
        Bonuses = bonuses;
    }

    public static BonusProgramReader Load(Guid id, decimal bonusAmount, string createdBy, string companyCode, DateTime? expires,
        string reason, List<BonusReader>? bonuses)
    {
        return new BonusProgramReader(id, bonusAmount, createdBy, companyCode, expires, reason, bonuses);
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