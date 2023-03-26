using AutoFixture;
using Calculator.Application.Features.Account.Commands.AccountSettlement;
using Calculator.Domain.Account.Snapshots;
using Calculator.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Tools;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class AccountSettlementCommandHandlerTests : CommandHandlerBaseTests<AccountSettlementCommandHandler,
    AccountSettlementCommand, Unit, Domain.Account.Account>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    {
        var request = Fixture.Create<AccountSettlementCommand>();
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var message =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(request, CancellationToken.None));

        Assert.Equal(AggregateNullMessageError("Account"), message.Message);
    }

    [Fact]
    public async Task Should_Remove_Recurring_Job_When_Account_Is_Not_Active()
    {
        var account = Fixture.GenerateAccount();
        account.Details.SetNewValue(nameof(account.Details.SettlementDayMonth), 10);

        var request = Fixture.Create<AccountSettlementCommand>();

        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);

        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Domain.Account.Account>()), Times.Once);
        AggregateRepositoryMock.Verify(
            v => v.UpdateWithSnapshotAsync<AccountSnapshot>(It.IsAny<Domain.Account.Account>()), Times.Never);
    }

    [Fact]
    public async Task Should_Call_UpdateWithSnapshotAsync_Method_When_Account_Is_Active()
    {
        var account = Fixture.GenerateAccount();

        account.Details.SetNewValue(nameof(account.Details.IsActive), true);
        account.Details.SetNewValue(nameof(account.Details.SettlementDayMonth), 10);

        var request = Fixture.Create<AccountSettlementCommand>();

        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);

        AggregateRepositoryMock.Verify(
            v => v.UpdateWithSnapshotAsync<AccountSnapshot>(It.IsAny<Domain.Account.Account>()), Times.Once);
        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Domain.Account.Account>()), Times.Never);
    }
}