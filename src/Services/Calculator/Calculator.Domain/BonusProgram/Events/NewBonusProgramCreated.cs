using Shared.Domain.Abstractions;

namespace Calculator.Domain.BonusProgram.Events;

public record NewBonusProgramCreated(decimal BonusAmount, string CreatedBy, string CompanyCode, DateTime? Expires,
    string Reason, Guid BonusId) : IEvent
{
    public static NewBonusProgramCreated Create(decimal bonusAmount, string createdBy, string companyCode,
        DateTime? expires, string reason, Guid bonusId)
    {
        return new NewBonusProgramCreated(bonusAmount, createdBy, companyCode, expires,
            reason, bonusId);
    }
}