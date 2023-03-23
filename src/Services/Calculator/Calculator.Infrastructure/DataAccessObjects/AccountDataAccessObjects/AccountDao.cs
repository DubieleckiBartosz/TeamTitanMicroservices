using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Infrastructure.DataAccessObjects.AccountDataAccessObjects;

public class AccountDao
{
    public Guid Id { get; init; }
    public decimal Balance { get; init; }
    public string AccountOwner { get; init; } = default!;
    public string CompanyCode { get; init; } = default!;
    public CountingType CountingType { get; init; }
    public AccountStatus AccountStatus { get; init; }
    public string? ActivatedBy { get; init; }
    public string CreatedBy { get; init; } = default!;
    public string? DeactivatedBy { get; init; }
    public bool IsActive { get; init; }
    public int WorkDayHours { get; init; }
    public decimal? HourlyRate { get; init; }
    public decimal? OvertimeRate { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public int? SettlementDayMonth { get; init; }
    public List<ProductItemDao> ProductItems { get; init; } = new();
    public List<WorkDayDao> WorkDays { get; init; } = new();
    public List<BonusDao> Bonuses { get; init; } = new();
    public List<SettlementDao> Settlements { get; init; } = new();

    public AccountReader Map()
    {
        var products = ProductItems.Select(_ =>
            ProductItemReader.Load(_.PieceworkProductId, _.Quantity, _.CurrentPrice, _.AccountId, _.IsConsidered,
                _.Date)).ToList();

        var workDays = WorkDays.Select(_ =>
            WorkDayReader.Create(_.Date, _.HoursWorked, _.Overtime, _.IsDayOff, _.CreatedBy, _.AccountId)).ToList();

        var bonuses = Bonuses.Select(_ =>
            BonusReader.Load(_.Id, _.BonusCode, _.Creator, _.Settled, _.Canceled, _.Created, _.Amount)).ToList();

        var settlements = Settlements.Select(_ => SettlementReader.Load(_.From, _.To, _.Value)).ToList();

        return AccountReader.Load(Id, AccountOwner, CompanyCode, CountingType,
            AccountStatus, ActivatedBy, CreatedBy, DeactivatedBy, IsActive,
            WorkDayHours, HourlyRate, OvertimeRate, Balance, ExpirationDate, SettlementDayMonth, products, workDays,
            bonuses, settlements);
    }
}