using Management.Application.Parameters.EmployeeParameters;
using Management.Application.ValueTypes;
using MediatR;

namespace Management.Application.Features.Commands.Contract.AddContract;

public record NewContractCommand(string Position, int EmployeeId,
    ContractType ContractType, SettlementType SettlementType, DateTime StartContract,
    DateTime? EndContract,
    int NumberHoursPerDay, int FreeDaysPerYear, string? BankAccountNumber, int PaymentMonthDay) : ICommand<Unit>
{
    public static NewContractCommand Create(NewContractParameters parameters)
    {
        var position = parameters.Position;
        var employeeId = parameters.EmployeeId;
        var contractType = parameters.ContractType;
        var settlementType = parameters.SettlementType; 
        var startContract = parameters.StartContract;
        var endContract = parameters.EndContract;
        var numberHoursPerDay = parameters.NumberHoursPerDay;
        var freeDaysPerYear = parameters.FreeDaysPerYear;
        var bankAccountNumber = parameters.BankAccountNumber;
        var paymentMonthDay = parameters.PaymentMonthDay;

        return new NewContractCommand(position, employeeId, contractType, settlementType, startContract,
            endContract, numberHoursPerDay, freeDaysPerYear, bankAccountNumber, paymentMonthDay);
    }
}