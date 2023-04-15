using AutoFixture;
using Management.Domain.Entities;
using Management.Domain.Types;
using Management.Domain.ValueObjects;

namespace Management.UnitTests.ModelGenerators;

public static class EntityGenerators
{
    public static Company GetCompany(this Fixture fixture)
    {
        var ownerId = fixture.Create<int>();
        var companyCode = fixture.Create<string>();
        var ownerCode = fixture.Create<string>();

        return Company.Init(ownerId, companyCode, ownerCode);
    }

    public static Company GetCompanyWithBaseData(
        this Fixture fixture,
        CompanyName? companyName = null,
        CommunicationData? communicationData = null)
    {
        var address = Address.Create(fixture.Create<string>(), fixture.Create<string>(),
            fixture.Create<string>(), fixture.Create<string>());

        var contact = Contact.Create(fixture.Create<string>(), fixture.Create<string>());
        companyName ??= CompanyName.Create(fixture.Create<string>());
        communicationData ??= CommunicationData.Create(address, contact);
        var company = fixture.GetCompany();

        company.UpdateData(companyName, null, communicationData);
        
        return company;
    }

    public static Company GetCompanyWithDepartments(this Fixture fixture, string? departmentName = null)
    {
        var company = fixture.GetCompanyWithBaseData();
        departmentName ??= fixture.Create<string>();

        company.AddNewDepartment(departmentName);

        return company;

    }

    public static Department GetDepartment(this Fixture fixture, string? name = null)
    {
        name ??= fixture.Create<string>();
        var departmentName = DepartmentName.Create(name);
        var newDepartment = Department.CreateDepartment(departmentName);

        return newDepartment;
    }

    public static Employee GetEmployee(this Fixture fixture,
        int? departmentId = null, string? leader = null, string? employeeCode = null,
        string? name = null, string? surname = null, DateTime? birthday = null,
        string? personIdentifier = null, CommunicationData? communicationData = null, bool withAccount = false)
    {
        departmentId ??= fixture.Create<int>();
        leader ??= fixture.Create<string>();
        employeeCode ??= fixture.Create<string>();
        name ??= fixture.Create<string>();
        surname ??= fixture.Create<string>();
        birthday ??= fixture.Create<DateTime>();
        personIdentifier ??= fixture.Create<string>();
        communicationData ??= fixture.Create<CommunicationData>();
        var employee = Employee.Create((int) departmentId, leader, employeeCode, name, surname, birthday.Value,
            personIdentifier, communicationData);

        if (withAccount)
        {
            var account = fixture.Create<Guid>();
            employee.AssignAccount(account);
        }

        return employee;
    }

    public static Employee GetEmployeeWithContract(this Fixture fixture, bool withAccount = false)
    {
        var employee = fixture.GetEmployee(withAccount: withAccount);
        var contract = fixture.GetContract();

        employee.AddContract(contract);

        return employee;
    }   
    
    public static Employee GetEmployeeWithDayOffRequests(this Fixture fixture, bool withAccount = false)
    {
        var employee = fixture.GetEmployee(withAccount: withAccount);
        var dayOffRequest = fixture.GetDayOffRequest();

        employee.AddDayOffRequest(dayOffRequest);

        return employee;
    }

    public static Contract GetContract(this Fixture fixture, string? position = null,
        ContractType? contractType = null, SettlementType? settlementType = null, TimeRange? timeRange = null,
        int? numberHoursPerDay = null, int? freeDaysPerYear = null, string? bankAccountNumber = null,
        string? createdBy = null, int? paymentMonthDay = null)
    { 
        var rnd = new Random();
        position ??= fixture.Create<string>();
        contractType ??= ContractType.ContractWork;
        settlementType ??= SettlementType.FixedSalary;
        if (timeRange == null)
        {
            var someFutureDate = DateTime.UtcNow.AddDays(rnd.Next(1, 30));
            timeRange ??= TimeRange.Create(someFutureDate, someFutureDate.AddMonths(3));
        }

        numberHoursPerDay ??= rnd.Next(1, 8);
        freeDaysPerYear ??= rnd.Next(1, 30);
        bankAccountNumber ??= fixture.Create<string>();
        createdBy ??= fixture.Create<string>();
        paymentMonthDay ??= rnd.Next(1, 28);

        return Contract.Create(position,
            contractType, settlementType, timeRange,
            numberHoursPerDay.Value, freeDaysPerYear.Value, bankAccountNumber,
            createdBy, paymentMonthDay.Value);
    }

    public static DayOffRequest GetDayOffRequest(this Fixture fixture, string? createdBy = null,
        RangeDaysOff? daysOff = null, ReasonType? reasonType = null,
        DayOffRequestDescription? description = null)
    {
        var rnd = new Random();

        createdBy ??= fixture.Create<string>();
        if (daysOff == null)
        {
            var someFutureDate = DateTime.UtcNow.AddDays(rnd.Next(1, 10));
            daysOff ??= RangeDaysOff.CreateRangeDaysOff(someFutureDate, someFutureDate.AddDays(3));
        }

        reasonType ??= ReasonType.Rest;
        description ??= DayOffRequestDescription.CreateDescription(fixture.Create<string>());

        return DayOffRequest.Create(createdBy, daysOff, reasonType, description);
    }
}