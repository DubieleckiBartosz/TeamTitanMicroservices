using Management.Application.Constants;
using Management.Application.Features.Commands.DayOffRequest.ConsiderDayOffRequest;
using Management.UnitTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.DayOffRequest;

public class ConsiderDayOffRequestHandlerTests : CommandHandlerBaseTests<ConsiderDayOffRequestHandler, ConsiderDayOffRequestCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_DayOffRequest_Not_Found()
    {
        var command = Fixture.GetConsiderDayOffRequestCommand();

        DayOffRequestRepositoryMock.Setup(_ => _.GetDayOffRequestByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("DayOffRequest"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetDayOffRequestByIdAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_ConsiderDayOffRequestAsync_Method_When_Day_Off_Request_Considered()
    {
        var command = Fixture.GetConsiderDayOffRequestCommand();
        var employee = Fixture.GetDayOffRequestDao();

        DayOffRequestRepositoryMock.Setup(_ => _.GetDayOffRequestByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => employee);

        await Handler.Handle(command, CancellationToken.None);

        DayOffRequestRepositoryMock.Verify(v => v.ConsiderDayOffRequestAsync(It.IsAny<Management.Domain.Entities.DayOffRequest>()), Times.Once);
    }
}