using Management.Application.Parameters.EmployeeParameters;
using Management.Application.ValueTypes;
using MediatR;

namespace Management.Application.Features.Commands.Employee.AddContract;

public record NewEmployeeContractCommand(string Position, int EmployeeId,
    ContractType ContractType, SettlementType SettlementType, decimal Salary, DateTime StartContract,
    DateTime? EndContract,
    int NumberHoursPerDay, int FreeDaysPerYear, string? BankAccountNumber, bool PaidIntoAccount, 
    decimal? HourlyRate, decimal? OvertimeRate, int PaymentMonthDay) : ICommand<Unit>
{
    public static NewEmployeeContractCommand Create(NewEmployeeContractParameters parameters)
    {
        var position = parameters.Position;
        var employeeId = parameters.EmployeeId;
        var contractType = parameters.ContractType;
        var settlementType = parameters.SettlementType;
        var salary = parameters.Salary;
        var startContract = parameters.StartContract;
        var endContract = parameters.EndContract;
        var numberHoursPerDay = parameters.NumberHoursPerDay;
        var freeDaysPerYear = parameters.FreeDaysPerYear;
        var bankAccountNumber = parameters.BankAccountNumber;
        var paidIntoAccount = parameters.PaidIntoAccount;
        var hourlyRate = parameters.HourlyRate;
        var overtimeRate = parameters.OvertimeRate;
        var paymentMonthDay = parameters.PaymentMonthDay;

        return new NewEmployeeContractCommand(position, employeeId, contractType, settlementType, salary, startContract,
            endContract, numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paidIntoAccount, hourlyRate,
            overtimeRate, paymentMonthDay);
    }
}