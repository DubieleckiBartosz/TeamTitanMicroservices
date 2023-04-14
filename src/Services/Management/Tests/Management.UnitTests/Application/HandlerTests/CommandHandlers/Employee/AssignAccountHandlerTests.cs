using Management.Application.Constants;
using Management.Application.Features.Commands.Employee.AssignAccount;
using Management.UnitTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Employee;

public class AssignAccountHandlerTests : CommandHandlerBaseTests<AssignCalculationAccountHandler, AssignCalculationAccountCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Employee_Not_Found()
    {
        var command = Fixture.GetAssignCalculationAccountCommand();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeNecessaryDataByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Employee"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetEmployeeNecessaryDataByCodeAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_AddAccountToEmployeeAsync_Method_When_Account_Assigned()
    {
        var command = Fixture.GetAssignCalculationAccountCommand();
        var employee = Fixture.GetEmployeeDao();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeNecessaryDataByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(() => employee);

        await Handler.Handle(command, CancellationToken.None);

        EmployeeRepositoryMock.Verify(v => v.AddAccountToEmployeeAsync(It.IsAny<Management.Domain.Entities.Employee>()), Times.Once);
    }
}