using Management.Application.Constants;
using Management.Application.Features.Commands.DayOffRequest.AddDayOffRequest;
using Management.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.DayOffRequest;

public class AddDayOffRequestHandlerTests : CommandHandlerBaseTests<DayOffRequestHandler, DayOffRequestCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Employee_Not_Found()
    {
        var command = Fixture.GetDayOffRequestCommand();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeWithDetailsByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Employee"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetEmployeeWithDetailsByCodeAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_CompleteAsync_Method_When_Day_Off_Request_Created()
    {
        var command = Fixture.GetDayOffRequestCommand();
        var employee = Fixture.GetEmployeeDaoDetails();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeWithDetailsByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(() => employee);

        await Handler.Handle(command, CancellationToken.None);

        UnitOfWork.Verify(v => v.CompleteAsync(It.IsAny<Domain.Entities.Employee>()), Times.Once);
    }
}