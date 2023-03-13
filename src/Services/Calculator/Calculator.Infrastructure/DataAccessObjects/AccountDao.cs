using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Infrastructure.DataAccessObjects;

public class AccountDao
{
    public Guid Id { get; init; }
    public string AccountOwner { get; init; }
    public string DepartmentCode { get; init; }
    public CountingType CountingType { get; init; }
    public AccountStatus AccountStatus { get; init; }
    public string? ActivatedBy { get; init; }
    public string CreatedBy { get; init; }
    public string? DeactivatedBy { get; init; }
    public bool IsActive { get; init; }
    public int WorkDayHours { get; init; }
    public decimal? HourlyRate { get; init; }
    public decimal? OvertimeRate { get; init; }
    public List<ProductItemDao> ProductItems { get; init; } = new();
    public List<WorkDayDao> WorkDays { get; init; } = new();

    public AccountReader Map()
    {
        var products = ProductItems.Select(_ =>
            ProductItemReader.Create(_.PieceworkProductId, _.Quantity, _.CurrentPrice, _.AccountId)).ToList();
        var workDays = WorkDays.Select(_ =>
            WorkDayReader.Create(_.Date, _.HoursWorked, _.Overtime, _.IsDayOff, _.CreatedBy, _.AccountId)).ToList();

        return AccountReader.Load(Id, AccountOwner, DepartmentCode, CountingType,
            AccountStatus, ActivatedBy, CreatedBy, DeactivatedBy, IsActive,
            WorkDayHours, HourlyRate, OvertimeRate, products, workDays);
    }
}