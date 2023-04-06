using Dapper;
using Management.Application.Contracts.Repositories;
using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;
using System.Data;

namespace Management.Infrastructure.Repositories;

public class EmployeeRepository : BaseRepository<EmployeeRepository>, IEmployeeRepository
{
    public EmployeeRepository(string dbConnection, ILoggerManager<EmployeeRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        var param = new DynamicParameters();

        param.Add("@employeeId", id);

        var result = (await QueryAsync<EmployeeDao>("employee_getEmployeeById_S", param, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task<Employee?> GetEmployeeByCodeAsync(string code)
    {
        var param = new DynamicParameters();

        param.Add("@code", code);

        var result = (await QueryAsync<EmployeeDao>("employee_getEmployeeByCode_S", param, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task AddAccountToEmployeeAsync(Employee employee)
    {
        var param = new DynamicParameters();

        param.Add("@employeeId", employee.Id);
        param.Add("@accountId", employee.AccountId);
         
        var result = await ExecuteAsync("employee_addAccountToEmployee_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'employee_addAccountToEmployee_U' failed", "Database Error");
        }
    }

    public async Task AddDayOffRequestToEmployeeAsync(Employee employee)
    {
        var dayOff = employee.DayOffRequests.Last();
        var param = new DynamicParameters(); 

        param.Add("@employeeId", employee.Id);
        param.Add("@createdBy", dayOff.CreatedBy);
        param.Add("@currentStatus", dayOff.CurrentStatus.Id);
        param.Add("@fromDate", dayOff.DaysOff.FromDate);
        param.Add("@toDate", dayOff.DaysOff.ToDate);
        param.Add("@reasonType", dayOff.ReasonType.Id);
        param.Add("@description", dayOff.Description?.ToString());
        param.Add("@canceled", dayOff.Canceled);

        var result = await ExecuteAsync("employee_addDayOffRequestToEmployee_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'employee_addDayOffRequestToEmployee_U' failed", "Database Error");
        }
    }

    public async Task AddContractToEmployeeAsync(Employee employee)
    {
        var contract = employee.Contracts.Last();
        var param = new DynamicParameters(); 

        param.Add("@employeeId", employee.Id);
        param.Add("@position", contract.Position);
        param.Add("@contractType", contract.ContractType.Id);
        param.Add("@settlementType", contract.SettlementType.Id);
        param.Add("@salary", contract.Salary);
        param.Add("@startContract", contract.TimeRange.StartContract);
        param.Add("@endContract", contract.TimeRange.EndContract);
        param.Add("@numberHoursPerDay", contract.NumberHoursPerDay);
        param.Add("@freeDaysPerYear", contract.FreeDaysPerYear);
        param.Add("@bankAccountNumber", contract.BankAccountNumber);
        param.Add("@paidIntoAccount", contract.PaidIntoAccount);
        param.Add("@hourlyRate", contract.HourlyRate);
        param.Add("@overtimeRate", contract.OvertimeRate);
        param.Add("@paymentMonthDay", contract.PaymentMonthDay); 

        var result = await ExecuteAsync("employee_addContractToEmployee_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'employee_addContractToEmployee_U' failed", "Database Error");
        }
    }

    public async Task UpdateContactDataAsync(Employee employee)
    {
        var param = new DynamicParameters();

        param.Add("@employeeId", employee.Id);

        var result = await ExecuteAsync("employee_contactData_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'employee_contactData_U' failed", "Database Error");
        }
    }

    public async Task UpdateAddressAsync(Employee employee)
    {
        var param = new DynamicParameters();

        param.Add("@employeeId", employee.Id);

        var result = await ExecuteAsync("employee_address_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'employee_address_U' failed", "Database Error");
        }
    }
}