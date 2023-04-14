using Management.Application.Constants;
using Management.Application.Features.Commands.Contract.UpdateDayHours;
using Management.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Contract;

public class UpdateDayHoursHandlerTests : CommandHandlerBaseTests<UpdateDayHoursHandler, UpdateDayHoursCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Contract_Not_Found()
    {
        var command = Fixture.GetUpdateDayHoursCommand();

        ContractRepositoryMock.Setup(_ => _.GetContractWithAccountByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Contract"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetContractWithAccountByIdAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_CompleteAsync_Method_When_Day_Hours_Updated()
    {
        var command = Fixture.GetUpdateDayHoursCommand();
        var contract = Fixture.GetContractWithAccountDao();

        ContractRepositoryMock.Setup(_ => _.GetContractWithAccountByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => contract);

        await Handler.Handle(command, CancellationToken.None);

        UnitOfWork.Verify(v => v.CompleteAsync(It.IsAny<Domain.Entities.Contract>()), Times.Once);
    }
}