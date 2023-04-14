using Management.Application.Constants;
using Management.Application.Features.Commands.Employee.UpdateEmployeeLeader;
using Management.UnitTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Employee;

public class UpdateEmployeeLeaderHandlerTests : CommandHandlerBaseTests<UpdateEmployeeLeaderHandler, UpdateEmployeeLeaderCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Employee_Not_Found()
    {
        var command = Fixture.GetUpdateEmployeeLeaderCommand();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeNecessaryDataByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Employee"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetEmployeeNecessaryDataByIdAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_UpdateLeaderAsync_Method_When_Leader_Changed()
    {
        var command = Fixture.GetUpdateEmployeeLeaderCommand();
        var employeeDao = Fixture.GetEmployeeDao();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeNecessaryDataByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => employeeDao);

        await Handler.Handle(command, CancellationToken.None);

        EmployeeRepositoryMock.Verify(v => v.UpdateLeaderAsync(It.IsAny<Management.Domain.Entities.Employee>()), Times.Once);
    }
}