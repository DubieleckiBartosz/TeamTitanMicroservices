using Dapper;
using Management.Application.Contracts.Repositories;
using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;
using System.Data;

namespace Management.Infrastructure.Repositories;

public class DepartmentRepository : BaseRepository<DepartmentRepository>, IDepartmentRepository 
{
    public DepartmentRepository(string dbConnection, ILoggerManager<DepartmentRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        var param = new DynamicParameters();

        param.Add("@departmentId", id);

        var result = (await QueryAsync<DepartmentDao>("department_getById_S", param, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task AddNewEmployeeAsync(Department department)
    { 
        var employee = department.Employees.Last();

        var param = new DynamicParameters();

        param.Add("@departmentId", department.Id);
        param.Add("@employeeCode", employee.EmployeeCode);
        param.Add("@name", employee.Name);
        param.Add("@surname", employee.Surname);
        param.Add("@birthday", employee.Birthday);
        param.Add("@personIdentifier", employee.PersonIdentifier);
        param.Add("@city", employee.CommunicationData.Address.City);
        param.Add("@street", employee.CommunicationData.Address.Street);
        param.Add("@numberStreet", employee.CommunicationData.Address.NumberStreet);
        param.Add("@postalCode", employee.CommunicationData.Address.PostalCode);       
        param.Add("@phoneNumber", employee.CommunicationData.Contact.PhoneNumber);       
        param.Add("@email", employee.CommunicationData.Contact.Email);

        var result = await ExecuteAsync("employee_newEmployee_I", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'department_newEmployee_I' failed", "Database Error");
        }
    }
}