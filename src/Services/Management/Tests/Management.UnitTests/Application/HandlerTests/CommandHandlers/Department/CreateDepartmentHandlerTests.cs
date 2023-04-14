using Management.Application.Constants;
using Management.Application.Features.Commands.Department.CreateDepartment;
using Management.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Department;

public class CreateDepartmentHandlerTests : CommandHandlerBaseTests<CreateDepartmentHandler, CreateDepartmentCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Company_Not_Found()
    {
        var command = Fixture.GetCreateDepartmentCommand();

        CompanyRepositoryMock.Setup(_ => _.GetCompanyWithDepartmentsByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Company"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetCompanyWithDepartmentsByCodeAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_AddNewDepartmentAsync_Method_When_Department_Created()
    {
        var command = Fixture.GetCreateDepartmentCommand();
        var company = Fixture.GetCompanyDao();

        CompanyRepositoryMock.Setup(_ => _.GetCompanyWithDepartmentsByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(() => company);

        await Handler.Handle(command, CancellationToken.None);

        CompanyRepositoryMock.Verify(v => v.AddNewDepartmentAsync(It.IsAny<Domain.Entities.Company>()), Times.Once);
    }
}