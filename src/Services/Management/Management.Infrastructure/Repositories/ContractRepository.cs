using System.Data;
using Dapper;
using Management.Application.Contracts.Repositories;
using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class ContractRepository : BaseRepository<ContractRepository>, IContractRepository
{
    public ContractRepository(string dbConnection, ILoggerManager<ContractRepository> loggerManager) : base(
        dbConnection, loggerManager)
    {
    }

    public async Task<ContractDao?> GetContractByIdAsync(int contractId)
    {
        var param = new DynamicParameters();

        param.Add("@contractId", contractId);

        var result =
            await QueryAsync<ContractDao>("contract_getById_S", param,
                CommandType.StoredProcedure);

        return result?.FirstOrDefault();
    }

    public async Task<ContractWithAccountDao?> GetContractWithAccountByIdAsync(int contractId)
    {
        var param = new DynamicParameters();

        param.Add("@contractId", contractId);

        var result =
            await QueryAsync<ContractWithAccountDao>("contract_getWithAccountById_S", param,
                CommandType.StoredProcedure);

        return result?.FirstOrDefault();
    }

    public async Task UpdateBankAccountNumberAsync(EmployeeContract contract)
    {
        var param = new DynamicParameters();

        param.Add("@contractId", contract.Id);
        param.Add("@bankAccountNumber", contract.BankAccountNumber); 
        param.Add("@version", contract.Version);

        var result = await ExecuteAsync("contract_bankAccountNumber_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'contract_bankAccountNumber_U' failed", "Database Error");
        }
    }

    public async Task UpdateSalaryAsync(EmployeeContract contract)
    {
        var param = new DynamicParameters();

        param.Add("@contractId", contract.Id);
        param.Add("@newSalary", contract.Salary);
        param.Add("@version", contract.Version);

        var result = await ExecuteAsync("contract_salary_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'contract_salary_U' failed", "Database Error");
        }
    }

    public async Task UpdatePaymentMonthDayAsync(EmployeeContract contract)
    {
        var param = new DynamicParameters();

        param.Add("@contractId", contract.Id);
        param.Add("@newPaymentMonthDay", contract.PaymentMonthDay);
        param.Add("@version", contract.Version);

        var result = await ExecuteAsync("contract_paymentMonthDay_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'dayOff_cancel_U' failed", "Database Error");
        }
    }

    public async Task UpdateHourlyRatesAsync(EmployeeContract contract)
    {
        var param = new DynamicParameters();

        param.Add("@contractId", contract.Id);
        param.Add("@salary", contract.Salary);
        param.Add("@newHourlyRate", contract.HourlyRate);
        param.Add("@newOvertimeRate", contract.OvertimeRate);
        param.Add("@version", contract.Version);

        var result = await ExecuteAsync("contract_financialData_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'contract_hourlyRates_U' failed", "Database Error");
        }
    }
     
    public async Task UpdateSettlementTypeAsync(EmployeeContract contract)
    {
        var param = new DynamicParameters();

        param.Add("@contractId", contract.Id);
        param.Add("@newSettlementType", contract.SettlementType.Id);
        param.Add("@version", contract.Version);

        var result = await ExecuteAsync("contract_settlementType_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'contract_settlementType_U' failed", "Database Error");
        }
    }
}