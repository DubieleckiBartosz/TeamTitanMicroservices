using Management.Domain.Entities;
using Management.Domain.Types;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Application.Models.DataAccessObjects;

public class ContractDao
{
    public int Id { get; init; }
    public int Version { get; init; }
    public string Position { get; init; } = default!;
    public string ContractNumber { get; init; } = default!;
    public int ContractType { get; init; }
    public int SettlementType { get; init; }
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

    public EmployeeContract Map()
    {
        var settlementType = Enumeration.GetById<SettlementType>(SettlementType);
        var contractType = Enumeration.GetById<ContractType>(ContractType);
        var timeRange = TimeRange.Create(StartContract, EndContract);

        var contract = EmployeeContract.Load(Id, Version, Position, contractType, settlementType, Salary, timeRange,
            NumberHoursPerDay, FreeDaysPerYear, BankAccountNumber, PaidIntoAccount, CreatedBy, HourlyRate, OvertimeRate,
            PaymentMonthDay);

        return contract;
    }
}