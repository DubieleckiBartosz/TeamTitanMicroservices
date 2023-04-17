using AutoFixture;
using Management.Domain.ValueObjects;
using Management.UnitTests.ModelGenerators;
using Shared.Domain.DomainExceptions;

namespace Management.UnitTests.Domain.Tests;

public class EmployeeTests : DomainBaseTests
{ 

    [Fact]
    public void Should_Assign_Account()
    {
        //Arrange
        var employee = Fixture.GetEmployee()!;
        var account = Fixture.Create<Guid>();

        var beforeAccount = employee.AccountId;

        //Act
        employee.AssignAccount(account);

        //Assert
        var afterAccount = employee.AccountId;
         
        Assert.Null(beforeAccount);
        Assert.NotNull(afterAccount);
    }

    [Fact]
    public void Should_Throw_Business_Exception_When_Contracts_Overlap()
    {
        //Arrange
        var employee = Fixture.GetEmployeeWithContract(true)!;
        var contract = Fixture.GetContract();

        //Act and Assert
        var responseException = Assert.Throws<BusinessException>(() => employee.AddContract(contract));
        Assert.Equal("Invalid time range contract".ToLower(), responseException.Title.ToLower());
    }

    [Fact]
    public void Should_Add_New_Event_When_New_Contract_Created()
    {         
        //Arrange
        var employee = Fixture.GetEmployeeWithContract(withAccount: true)!;

        var now = DateTime.UtcNow;
        var timeRange = TimeRange.Create(now.AddMonths(6), now.AddMonths(9));
        var contract = Fixture.GetContract(timeRange: timeRange);

        var eventsCountBefore = employee.Events.Count();

        //Act
        employee.AddContract(contract);
        
        //Assert
        var eventsCountAfter = employee.Events.Count();
        Assert.Equal(eventsCountBefore + 1, eventsCountAfter);
    }

    [Fact]
    public void Should_Throw_Business_Exception_When_Days_Off_Overlap()
    {
        //Arrange
        var employee = Fixture.GetEmployeeWithDayOffRequests()!;
        var dayOffRequest = Fixture.GetDayOffRequest(daysOff: employee.DayOffRequests.First().DaysOff);

        //Act and Assert
        var responseException = Assert.Throws<BusinessException>(() => employee.AddDayOffRequest(dayOffRequest));
        Assert.Equal("Invalid range days".ToLower(), responseException.Title.ToLower());
    }

    [Fact]
    public void Should_Add_New_Event_When_New_DayOffRequest_Created()
    {
        //Arrange
        var employee = Fixture.GetEmployeeWithDayOffRequests()!;

        var now = DateTime.UtcNow;
        var daysOff = RangeDaysOff.CreateRangeDaysOff(now.AddDays(20), now.AddDays(25));

        var dayOffRequest = Fixture.GetDayOffRequest(daysOff: daysOff);
         
        var eventsCountBefore = employee.Events.Count();

        //Act
        employee.AddDayOffRequest(dayOffRequest);

        //Assert
        var eventsCountAfter = employee.Events.Count();
        Assert.Equal(eventsCountBefore + 1, eventsCountAfter);
    }

    [Fact]
    public void Should_Increment_Version_When_Address_Updated()
    {
        //Arrange
        var employee = Fixture.GetEmployee();
        var versionBefore = employee.Version;
        var city = Fixture.Create<string>();
        var street = Fixture.Create<string>();
        var numberStreet = Fixture.Create<string>();
        var postalCode = Fixture.Create<string>();
        //Act
        employee.UpdateAddress(city, street, numberStreet, postalCode);

        //Assert
        var versionAfter = employee.Version;

        Assert.Equal(versionBefore + 1, versionAfter);
    }

    [Fact]
    public void Should_Increment_Version_When_Contact_Updated()
    {
        //Arrange
        var employee = Fixture.GetEmployee();
        var versionBefore = employee.Version;
        var phone = Fixture.Create<string>();
        var mail = Fixture.Create<string>();

        //Act
        employee.UpdateContact(phone, mail);

        //Assert
        var versionAfter = employee.Version;

        Assert.Equal(versionBefore + 1, versionAfter);
    }

    [Fact]
    public void Should_Increment_Version_When_Leader_Changed()
    {
        //Arrange
        var employee = Fixture.GetEmployee();
        var versionBefore = employee.Version; 
        var newLeader = Fixture.Create<string>();

        //Act
        employee.UpdateLeader(newLeader);

        //Assert
        var versionAfter = employee.Version;

        Assert.Equal(versionBefore + 1, versionAfter);
    }
}