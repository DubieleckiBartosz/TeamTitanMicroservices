using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Infrastructure.DataAccessObjects;

public class AccountDao
{
    public Guid Id { get; init; }
    public string AccountOwner { get; init; }
    public string DepartmentCode { get; init; }
    public CountingType CountingType { get; init; }
    public AccountStatus AccountStatus { get; private set; }
    public string? ActivatedBy { get; init; }
    public string CreatedBy { get; init; }
    public string? DeactivatedBy { get; init; }
    public bool IsActive { get; init; }
    public int WorkDayHours { get; init; }
    public decimal? HourlyRate { get; init; }
    public decimal? OvertimeRate { get; init; }
    public List<ProductItemDao> ProductItems { get; private set; } = new List<ProductItemDao>();
    public List<WorkDayDao> WorkDays { get; private set; } = new List<WorkDayDao>();

}