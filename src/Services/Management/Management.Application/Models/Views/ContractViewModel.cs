using Management.Application.ValueTypes;

namespace Management.Application.Models.Views;

public class ContractViewModel
{
    public string Position { get; init; } = default!;
    public string ContractNumber { get; init; } = default!;
    public ContractType ContractType { get; init; }
    public SettlementType SettlementType { get; init; }
    public decimal Salary { get; init; }
    public decimal? HourlyRate { get; init; }
    public decimal? OvertimeRate { get; init; }
    public DateTime StartContract { get; init; }
    public DateTime? EndContract { get; init; }
    public int NumberHoursPerDay { get; init; }
    public int FreeDaysPerYear { get; init; }
    public string? BankAccountNumber { get; init; }
    public bool PaidIntoAccount { get; init; }
    public int PaymentMonthDay { get; init; }
    public string CreatedBy { get; init; } = default!;
}