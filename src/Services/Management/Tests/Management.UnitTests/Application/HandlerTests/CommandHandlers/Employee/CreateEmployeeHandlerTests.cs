using Management.Application.Constants;
using Management.Application.Features.Commands.Employee.CreateEmployee;
using Management.UnitTests.ModelGenerators;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Employee;

public class CreateEmployeeHandlerTests : CommandHandlerBaseTests<CreateEmployeeHandler, CreateEmployeeCommand, string>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Department_Not_Found()
    {
        var command = Fixture.GetCreateEmployeeCommand();

        DepartmentRepositoryMock.Setup(_ => _.GetDepartmentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Department"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetDepartmentByIdAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_CompleteAsync_Method_When_Employee_Created_And_Should_Return_Some_Value()
    {
        var command = Fixture.GetCreateEmployeeCommand();
        var department = Fixture.GetDepartmentDao();

        DepartmentRepositoryMock.Setup(_ => _.GetDepartmentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => department);

        var result = await Handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        UnitOfWork.Verify(v => v.CompleteAsync(It.IsAny<Management.Domain.Entities.Department>()), Times.Once);
    }
}