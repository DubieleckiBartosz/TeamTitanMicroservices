using AutoFixture;
using Management.Domain.Entities;
using Management.Domain.ValueObjects;
using Management.UnitTests.ModelGenerators;
using Shared.Domain.DomainExceptions;

namespace Management.UnitTests.Domain.Tests;

public class CompanyTests : BaseDomainTests
{
    private readonly string _city;
    private readonly string _street;
    private readonly string _numberStreet;
    private readonly string _postalCode;
    private readonly string _phoneNumber;
    private readonly string _email;

    public CompanyTests()
    {
        _city = Fixture.Create<string>();
        _street = Fixture.Create<string>();
        _numberStreet = Fixture.Create<string>();
        _postalCode = Fixture.Create<string>();
        _phoneNumber = Fixture.Create<string>();
        _email = Fixture.Create<string>();
    }

    [Fact]
    public void Should_Add_New_Event_When_Company_Initiated()
    {
        //Act
        var company = Fixture.GetCompany();

        //Assert
        var eventsCount = company.Events.Count;

        Assert.Equal(1, eventsCount);
    }

    [Fact]
    public void Should_Throw_Business_Exception_When_Department_Already_Exists()
    {
        //Arrange
        var departmentName = Fixture.Create<string>();
        var company = Fixture.GetCompanyWithDepartments(departmentName);

        //Act and Assert
        var resultException = Assert.Throws<BusinessException>(() => company.AddNewDepartment(departmentName));

        Assert.Equal("Duplicate name", resultException.Title);
    }

    [Fact]
    public void Should_Add_New_Department_To_Company()
    {
        //Arrange
        var departmentName = Fixture.Create<string>();
        var company = Fixture.GetCompanyWithDepartments();
        var departmentsCountBefore = company.Departments.Count;
        
        //Act
        company.AddNewDepartment(departmentName);
        var departmentsCountAfter = company.Departments.Count;

        //Assert
        Assert.Equal(departmentsCountBefore + 1, departmentsCountAfter);
    }

    [Fact]
    public void Should_Increment_Version_When_Data_Updated()
    {
        //Arrange 
        var contact = Contact.Create(_phoneNumber, _email);
        var address = Address.Create(_city, _street, _numberStreet, _postalCode);
        var companyName = CompanyName.Create(Fixture.Create<string>());
        var communicationData = CommunicationData.Create(address, contact);
        var company = Fixture.GetCompany();

        var beforeVersion = company.Version;

        //Act
        company.UpdateData(companyName, null, communicationData);

        //Assert
        var afterVersion = company.Version;

        Assert.Equal(beforeVersion + 1, afterVersion);
    }

    [Fact]
    public void Should_Update_CommunicationData()
    {
        //Arrange
        var company = Fixture.GetCompanyWithBaseData();
        var streetBefore = company.CommunicationData!.Address.Street;
        var phoneBefore = company.CommunicationData.Contact.PhoneNumber;
        //Act
        company.UpdateCommunicationData(_phoneNumber,_email, _city, _street,
            _numberStreet, _postalCode);

        //Assert
        var streetAfter = company.CommunicationData!.Address.Street;
        var phoneAfter = company.CommunicationData.Contact.PhoneNumber;

        Assert.NotEqual(streetBefore, streetAfter);
        Assert.NotEqual(phoneBefore, phoneAfter);
    }
}