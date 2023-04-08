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

    public async Task<EmployeeDao?> GetEmployeeNecessaryDataByIdAsync(int id)
    {
        var param = new DynamicParameters();

        param.Add("@employeeId", id);

        var result =
            await this.QueryAsync<EmployeeDao>("employee_getNecessaryDataById_S", param, CommandType.StoredProcedure);

        return result?.FirstOrDefault();
    }

    public async Task<EmployeeDao?> GetEmployeeWithDetailsByIdAsync(int id)
    {
        var dict = new Dictionary<int, EmployeeDao>();
        var param = new DynamicParameters();

        param.Add("@employeeId", id);

        var result = (await QueryAsync<EmployeeDao, CommunicationDao?, ContractDao?, DayOffRequestDao?, EmployeeDao>(
            "employee_getEmployeeById_S",
            (e, cd, ec, dor) =>
            {
                if (!dict.TryGetValue(e.Id, out var value))
                {
                    value = e;
                    value.Communication = cd;
                    dict.Add(e.Id, value);
                }

                if (ec != null)
                {
                    value.Contracts.Add(ec);
                }

                if (dor != null)
                {
                    value.DayOffRequests.Add(dor);
                }

                return value;
            }, "Id,Id,Id", param, CommandType.StoredProcedure)).FirstOrDefault();

        return result;
    }

    public async Task<EmployeeDao?> GetEmployeeWithDetailsByCodeAsync(string code)
    {
        var dict = new Dictionary<int, EmployeeDao>();

        var param = new DynamicParameters();

        param.Add("@code", code);

        var result = (await QueryAsync<EmployeeDao, CommunicationDao, ContractDao?, DayOffRequestDao?, EmployeeDao>(
            "employee_getEmployeeByCode_S",
            (e, cd, ec, dor) =>
            {
                if (!dict.TryGetValue(e.Id, out var value))
                {
                    value = e;
                    value.Communication = cd;
                    dict.Add(e.Id, value);
                }

                if (ec != null)
                {
                    value.Contracts.Add(ec);
                }

                if (dor != null)
                {
                    value.DayOffRequests.Add(dor);
                }

                return value;
            }, splitOn: "Id,Id,Id", param, CommandType.StoredProcedure)).FirstOrDefault();
        return result;
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

        var result = await ExecuteAsync("dayOff_newDayOffRequest_I", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'dayOff_newDayOffRequest_I' failed", "Database Error");
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
        param.Add("@createdBy", contract.CreatedBy);
        param.Add("@paymentMonthDay", contract.PaymentMonthDay); 

        var result = await ExecuteAsync("contract_newContract_I", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'contract_newContract_I' failed", "Database Error");
        }
    }

    public async Task UpdateContactDataAsync(Employee employee)
    {
        var param = new DynamicParameters();

        param.Add("@employeeId", employee.Id); 
        param.Add("@phoneNumber", employee.CommunicationData.Contact.PhoneNumber);
        param.Add("@email", employee.CommunicationData.Contact.Email);

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
        param.Add("@city", employee.CommunicationData.Address.City);
        param.Add("@street", employee.CommunicationData.Address.Street);
        param.Add("@numberStreet", employee.CommunicationData.Address.NumberStreet);
        param.Add("@postalCode", employee.CommunicationData.Address.PostalCode);

        var result = await ExecuteAsync("employee_address_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'employee_address_U' failed", "Database Error");
        }
    }

    public async Task UpdateLeaderAsync(Employee employee)
    {
        var param = new DynamicParameters();

        param.Add("@employeeId", employee.Id);
        param.Add("@newLeader", employee.Leader); 

        var result = await ExecuteAsync("employee_newLeader_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'employee_newLeader_U' failed", "Database Error");
        }
    }
}