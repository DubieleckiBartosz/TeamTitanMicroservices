using Calculator.Application.ReadModels.BonusReaders;

namespace Calculator.Infrastructure.DataAccessObjects;

public class BonusProgramDao
{
    public Guid Id { get; init; }
    public decimal BonusAmount { get; init; }
    public string CreatedBy { get; init; }
    public string CompanyCode { get; init; }
    public DateTime? Expires { get; init; }
    public string Reason { get; init; }
    public List<BonusDao>? Bonuses { get; init; }

    public BonusProgramReader Map()
    {
        var bonuses = Bonuses?.Select(_ => BonusReader.Load(_.Id, _.BonusCode, _.GroupBonus, _.Recipient, _.Creator,
                _.Settled, _.Canceled, _.Created))
            .ToList();
        return BonusProgramReader.Load(Id, BonusAmount, CreatedBy, CompanyCode, Expires, Reason, bonuses);
    }
}