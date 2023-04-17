using AutoFixture;
using Calculator.Application.Features.Account.Commands.AddPieceProduct;
using Calculator.Application.Parameters.AccountParameters;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class AddPieceProductHandlerTests : CommandHandlerBaseTests<AddPieceProductCommandHandler, AddPieceProductCommand
    , Unit, Calculator.Domain.Account.Account>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    {
        var parameters = Fixture.Create<AddPieceProductParameters>();
        var request = AddPieceProductCommand.Create(parameters);
        AggregateRepositoryMock.Setup(_ => _.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var message =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(request, CancellationToken.None));

        Assert.Equal(AggregateNullMessageError("Account"), message.Message);
    }
}