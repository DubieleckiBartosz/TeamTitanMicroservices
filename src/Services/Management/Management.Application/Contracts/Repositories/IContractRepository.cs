using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IContractRepository
{
    Task<ContractDao?> GetContractByIdAsync(int contractId); 
    Task<ContractWithAccountDao?> GetContractWithAccountByIdAsync(int contractId);
    Task UpdateBankAccountNumberAsync(EmployeeContract contract);
    Task UpdateSalaryAsync(EmployeeContract contract);
    Task UpdatePaymentMonthDayAsync(EmployeeContract contract);
    Task UpdateHourlyRatesAsync(EmployeeContract contract);
    Task UpdateSettlementTypeAsync(EmployeeContract contract);
}