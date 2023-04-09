using Management.Application.ValueTypes;
using Newtonsoft.Json;

namespace Management.Application.Parameters.EmployeeParameters;

public class NewContractParameters
{
    public string Position { get; init; } = default!;
    public int EmployeeId { get; init; } 
    public ContractType ContractType { get; init; }
    public SettlementType SettlementType { get; init; }
    public decimal Salary { get; init; }
    public DateTime StartContract { get; init; }
    public DateTime? EndContract { get; init; }
    public int NumberHoursPerDay { get; init; }
    public int FreeDaysPerYear { get; init; }
    public string? BankAccountNumber { get; init; } 
    public decimal? HourlyRate { get; init; }
    public decimal? OvertimeRate { get; init; }
    public int PaymentMonthDay { get; init; }


    /// <summary>
    /// For tests
    /// </summary>
    public NewContractParameters()
    {
    }

    [JsonConstructor]
    public NewContractParameters(string position, int employeeId, ContractType contractType,
        SettlementType settlementType, decimal salary, DateTime startContract, DateTime? endContract,
        int numberHoursPerDay, int freeDaysPerYear, string? bankAccountNumber, 
        decimal? hourlyRate, decimal? overtimeRate, int paymentMonthDay)
    {
        Position = position;
        EmployeeId = employeeId;
        ContractType = contractType;
        SettlementType = settlementType;
        Salary = salary;
        StartContract = startContract;
        EndContract = endContract;
        NumberHoursPerDay = numberHoursPerDay;
        FreeDaysPerYear = freeDaysPerYear;
        BankAccountNumber = bankAccountNumber; 
        HourlyRate = hourlyRate;
        OvertimeRate = overtimeRate;
        PaymentMonthDay = paymentMonthDay;
    }
}