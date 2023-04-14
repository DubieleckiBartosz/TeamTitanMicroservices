using AutoFixture;
using Management.Application.Models.DataAccessObjects;

namespace Management.UnitTests.ModelGenerators;

public static class DaoGenerators
{
    public static CompanyDao GetCompanyDao(this Fixture fixture)
    {
        var rnd = new Random();

        var departments = new List<DepartmentDao>()
        {
            fixture.GetDepartmentDao()
        };

        var companyDao = fixture.Build<CompanyDao>()
            .With(w => w.CompanyStatus, rnd.Next(1, 4))
            .With(w => w.Departments, departments).Create();

        return companyDao;
    }

    public static DepartmentDao GetDepartmentDao(this Fixture fixture)
    {
        var employees = new List<EmployeeDao>()
        {
            fixture.GetEmployeeDaoDetails()
        };

        var department = fixture.Build<DepartmentDao>()
            .With(w => w.Employees, employees)
            .Create();

        return department;
    }
    public static EmployeeDao GetEmployeeDao(this Fixture fixture, bool withCommunication = false)
    {
        var creator = fixture.Build<EmployeeDao>()
            .Without(w => w.DayOffRequests)
            .Without(w => w.Contracts);

        var employee = withCommunication
            ? creator.Create()
            : creator.Without(w => w.Communication)
            .Create();

        return employee;
    }

    public static EmployeeDao GetEmployeeDaoDetails(this Fixture fixture)
    {
        var contracts = new List<ContractDao>()
        {
            fixture.GetContractDao()
        };

        var dayOffRequests = new List<DayOffRequestDao>()
        {
            fixture.GetDayOffRequestDao()
        };

        var employee = fixture.Build<EmployeeDao>()
            .With(w => w.DayOffRequests, dayOffRequests)
            .With(w => w.Contracts, contracts).Create();

        return employee;
    }

    public static EmployeeDao GetEmployeeDaoWithContracts(this Fixture fixture)
    {
        var contracts = new List<ContractDao>()
        {
            fixture.GetContractDao()
        };

        var employee = fixture.Build<EmployeeDao>()
            .With(w => w.Contracts, contracts)
            .Without(w => w.DayOffRequests)
            .Create();

        return employee;
    }

    public static EmployeeDao GetEmployeeDaoWithDayOffRequests(this Fixture fixture)
    {
        var dayOffRequests = new List<DayOffRequestDao>()
        {
            fixture.GetDayOffRequestDao()
        };

        var employee = fixture.Build<EmployeeDao>()
            .With(w => w.DayOffRequests, dayOffRequests)
            .Without(w => w.Contracts).Create();

        return employee;
    }

    public static ContractDao GetContractDao(this Fixture fixture)
    {
        var rnd = new Random();

        var now = DateTime.UtcNow;
        var contract = fixture.Build<ContractDao>()
            .With(w => w.SettlementType, rnd.Next(1, 3))
            .With(w => w.ContractType, rnd.Next(1, 3))
            .With(w => w.StartContract, now)
            .With(w => w.EndContract, now.AddMonths(1)).Create();

        return contract;
    }
    public static ContractWithAccountDao GetContractWithAccountDao(this Fixture fixture)
    {
        var rnd = new Random();

        var now = DateTime.UtcNow;
        var contract = fixture.Build<ContractWithAccountDao>()
            .With(w => w.SettlementType, rnd.Next(1, 3))
            .With(w => w.ContractType, rnd.Next(1, 3))
            .With(w => w.StartContract, now)
            .With(w => w.EndContract, now.AddMonths(1)).Create();

        return contract;
    }

    public static DayOffRequestDao GetDayOffRequestDao(this Fixture fixture, int status = 1)
    {
        var rnd = new Random();

        var now = DateTime.UtcNow;
        var dayOffRequest = fixture.Build<DayOffRequestDao>()
            .With(w => w.ReasonType, rnd.Next(1, 5))
            .With(w => w.CurrentStatus, status)
            .With(w => w.FromDate, now.AddDays(1))
            .With(w => w.ToDate, now.AddDays(2)).Create();

        return dayOffRequest;
    }
}