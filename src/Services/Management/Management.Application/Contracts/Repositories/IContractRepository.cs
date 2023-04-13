using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IContractRepository
{
    Task<ContractDao?> GetContractByIdAsync(int contractId); 
    Task<ContractWithAccountDao?> GetContractWithAccountByIdAsync(int contractId);
    Task UpdateBankAccountNumberAsync(Contract contract);
    Task UpdateSalaryAsync(Contract contract);
    Task UpdatePaymentMonthDayAsync(Contract contract);
    Task UpdateHourlyRatesAsync(Contract contract);
    Task UpdateSettlementTypeAsync(Contract contract);
    Task UpdateDayHoursAsync(Contract contract);
}