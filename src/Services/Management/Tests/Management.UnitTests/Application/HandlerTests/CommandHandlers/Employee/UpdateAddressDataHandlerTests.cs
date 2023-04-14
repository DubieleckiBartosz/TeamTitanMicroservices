using Management.Application.Constants;
using Management.Application.Features.Commands.Employee.UpdateAddressData;
using Management.UnitTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Employee;

public class UpdateAddressDataHandlerTests : CommandHandlerBaseTests<UpdateAddressDataHandler, UpdateAddressDataCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Employee_Not_Found()
    {
        var command = Fixture.GetUpdateAddressDataCommand();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeWithCommunicationDataByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Employee"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetEmployeeWithCommunicationDataByIdAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_UpdateAddressAsync_Method_When_Address_Updated()
    {
        var command = Fixture.GetUpdateAddressDataCommand();
        var employeeDao = Fixture.GetEmployeeDao(true);

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeWithCommunicationDataByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => employeeDao);

        await Handler.Handle(command, CancellationToken.None);

        EmployeeRepositoryMock.Verify(v => v.UpdateAddressAsync(It.IsAny<Management.Domain.Entities.Employee>()), Times.Once);
    }
}