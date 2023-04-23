using AutoFixture;
using Calculator.Application.Features.Account.Commands.ChangeCountingType;
using Calculator.Domain.Account.Snapshots;
using Calculator.Domain.Types;
using Calculator.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ChangeCountingTypeHandlerTests : CommandHandlerBaseTests<ChangeCountingTypeCommandHandler,
    ChangeCountingTypeCommand, Unit, Calculator.Domain.Account.Account>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    {
        var newCountingType = Fixture.Create<CountingType>();
        var request = ChangeCountingTypeCommand.Create(newCountingType, Guid.NewGuid());
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var message =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(request, CancellationToken.None));

        Assert.Equal(AggregateNullMessageError("Account"), message.Message);
    }

    [Fact]
    public async Task Should_Change_Counting_Type()
    {
        var account = Fixture.GenerateAccountWithBaseData();

        var newCountingType = Fixture.Create<CountingType>();
        var request = ChangeCountingTypeCommand.Create(newCountingType, account.Id);

        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

       await Handler.Handle(request, CancellationToken.None);
       AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Calculator.Domain.Account.Account>()), Times.Once); 
    } 
}