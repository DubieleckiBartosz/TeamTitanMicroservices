using Management.Application.Constants;
using Management.Application.Features.Commands.Employee.UpdateContactData;
using Management.UnitTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Employee;

public class UpdateContactDataHandlerTests : CommandHandlerBaseTests<UpdateContactDataHandler, UpdateContactDataCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Employee_Not_Found()
    {
        var command = Fixture.GetUpdateContactDataCommand();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeWithCommunicationDataByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Employee"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetEmployeeWithCommunicationDataByIdAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_UpdateContactDataAsync_Method_When_Contact_Updated()
    {
        var command = Fixture.GetUpdateContactDataCommand();
        var employeeDao = Fixture.GetEmployeeDao(true);

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeWithCommunicationDataByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => employeeDao);

        await Handler.Handle(command, CancellationToken.None);

        EmployeeRepositoryMock.Verify(v => v.UpdateContactDataAsync(It.IsAny<Management.Domain.Entities.Employee>()), Times.Once);
    }
}