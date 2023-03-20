using Calculator.Domain.Statuses;
using Calculator.Domain.Types;

namespace Calculator.Application.Features.Account.ViewModels;

public class AccountViewModel
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
}