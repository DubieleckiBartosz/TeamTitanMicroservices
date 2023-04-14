using AutoFixture;
using Management.Application.Features.Commands.Company.CompleteData;
using Management.Application.Features.Commands.Company.InitCompany;
using Management.Application.Features.Commands.Company.UpdateContact;
using Management.Application.Features.Commands.Contract.AddContract;
using Management.Application.Features.Commands.Contract.UpdateBankAccount;
using Management.Application.Features.Commands.Contract.UpdateDayHours;
using Management.Application.Features.Commands.Contract.UpdateFinancialData;
using Management.Application.Features.Commands.Contract.UpdatePaymentMonthDay;
using Management.Application.Features.Commands.Contract.UpdateSalary;
using Management.Application.Features.Commands.Contract.UpdateSettlementType;
using Management.Application.Features.Commands.DayOffRequest.AddDayOffRequest;
using Management.Application.Features.Commands.DayOffRequest.CancelDayOffRequest;
using Management.Application.Features.Commands.DayOffRequest.ConsiderDayOffRequest;
using Management.Application.Features.Commands.Department.CreateDepartment;
using Management.Application.Features.Commands.Employee.AssignAccount;
using Management.Application.Features.Commands.Employee.CreateEmployee;
using Management.Application.Features.Commands.Employee.UpdateAddressData;
using Management.Application.Features.Commands.Employee.UpdateContactData;
using Management.Application.Features.Commands.Employee.UpdateEmployeeLeader;
using Management.Application.Parameters.CompanyParameters;
using Management.Application.Parameters.ContractParameters;
using Management.Application.Parameters.DayOffRequestParameters;
using Management.Application.Parameters.DepartmentParameters;
using Management.Application.Parameters.EmployeeParameters;

namespace Management.UnitTests.Application.HandlerTests.ModelGenerators;

public static class CommandGenerators
{
    public static CompleteDataCommand GetCompleteDataCommand(this Fixture fixture)
    {
        var parameters = fixture.Create<CompleteDataParameters>();
        return CompleteDataCommand.Create(parameters);
    }

    public static InitCompanyCommand GetInitCompanyCommand(this Fixture fixture)
    {
        return new InitCompanyCommand();
    }

    public static UpdateCompanyContactCommand GetUpdateCompanyContactCommand(this Fixture fixture)
    {
        var parameters = fixture.Create<UpdateCompanyContactParameters>();
        return UpdateCompanyContactCommand.Create(parameters);
    } 

    public static NewContractCommand GetNewContractCommand(this Fixture fixture)
    {
        var dateUtcNow = DateTime.UtcNow;
        var parameters = fixture.Build<NewContractParameters>()
            .With(w => w.StartContract, dateUtcNow.AddMonths(2))
            .With(w => w.EndContract, dateUtcNow.AddMonths(3)).Create();

        return NewContractCommand.Create(parameters);
    }

    public static UpdateBankAccountCommand GetUpdateBankAccountCommand(this Fixture fixture)
    { 
        var parameters = fixture.Build<UpdateBankAccountParameters>().Create();
        return UpdateBankAccountCommand.Create(parameters);
    }

    public static UpdateDayHoursCommand GetUpdateDayHoursCommand(this Fixture fixture)
    { 
        var parameters = fixture.Build<UpdateDayHoursParameters>().Create();
        return UpdateDayHoursCommand.Create(parameters);
    }
    public static UpdateFinancialDataCommand GetUpdateFinancialDataCommand(this Fixture fixture)
    { 
        var parameters = fixture.Build<UpdateFinancialDataParameters>().Create();
        return UpdateFinancialDataCommand.Create(parameters);
    }
    
    public static UpdatePaymentMonthDayCommand GetUpdatePaymentMonthDayCommand(this Fixture fixture)
    { 
        var parameters = fixture.Build<UpdatePaymentMonthDayParameters>().Create();
        return UpdatePaymentMonthDayCommand.Create(parameters);
    }
    
    public static UpdateSalaryCommand GetUpdateSalaryCommand(this Fixture fixture)
    { 
        var parameters = fixture.Build<UpdateSalaryParameters>().Create();
        return UpdateSalaryCommand.Create(parameters);
    }
    
    public static UpdateSettlementTypeCommand GetUpdateSettlementTypeCommand(this Fixture fixture)
    { 
        var parameters = fixture.Build<UpdateSettlementTypeParameters>().Create();
        return UpdateSettlementTypeCommand.Create(parameters);
    }

    public static DayOffRequestCommand GetDayOffRequestCommand(this Fixture fixture)
    {
        var dateUtcNow = DateTime.UtcNow;
        var parameters = fixture.Build<NewDayOffRequestParameters>()
            .With(w => w.From, dateUtcNow.AddMonths(6))
            .With(w => w.To, dateUtcNow.AddMonths(7)).Create();

        return DayOffRequestCommand.Create(parameters);
    }
    
    public static CancelDayOffRequestCommand GetCancelDayOffRequestCommand(this Fixture fixture)
    { 
        var parameters = fixture.Create<CancelDayOffRequestParameters>();

        return CancelDayOffRequestCommand.Create(parameters);
    }
    
    public static ConsiderDayOffRequestCommand GetConsiderDayOffRequestCommand(this Fixture fixture)
    { 
        var parameters = fixture.Create<ConsiderDayOffRequestParameters>();

        return ConsiderDayOffRequestCommand.Create(parameters);
    }
    
    public static CreateDepartmentCommand GetCreateDepartmentCommand(this Fixture fixture)
    { 
        var parameters = fixture.Create<CreateDepartmentParameters>();

        return CreateDepartmentCommand.Create(parameters);
    }
    
    public static AssignCalculationAccountCommand GetAssignCalculationAccountCommand(this Fixture fixture)
    {
        var ownerVerificationCode = fixture.Create<string>();
        var accountId = fixture.Create<Guid>();

        return AssignCalculationAccountCommand.Create(ownerVerificationCode, accountId);
    }
    
    public static CreateEmployeeCommand GetCreateEmployeeCommand(this Fixture fixture)
    {
        var parameters = fixture.Create<CreateEmployeeParameters>();

        return CreateEmployeeCommand.Create(parameters);
    }
    
    public static UpdateAddressDataCommand GetUpdateAddressDataCommand(this Fixture fixture)
    {
        var parameters = fixture.Create<UpdateAddressDataParameters>();

        return UpdateAddressDataCommand.Create(parameters);
    }
    
    public static UpdateContactDataCommand GetUpdateContactDataCommand(this Fixture fixture)
    {
        var parameters = fixture.Create<UpdateContactDataParameters>();

        return UpdateContactDataCommand.Create(parameters);
    }
    
    public static UpdateEmployeeLeaderCommand GetUpdateEmployeeLeaderCommand(this Fixture fixture)
    {
        var parameters = fixture.Create<UpdateEmployeeLeaderParameters>();

        return UpdateEmployeeLeaderCommand.Create(parameters);
    }
}