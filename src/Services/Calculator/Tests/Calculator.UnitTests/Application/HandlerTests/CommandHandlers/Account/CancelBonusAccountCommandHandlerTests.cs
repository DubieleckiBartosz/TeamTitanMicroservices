using AutoFixture;
using Calculator.Application.Features.Account.Commands.CancelBonus;
using Calculator.Application.Parameters.AccountParameters;
using Calculator.Domain.Account.Snapshots;
using Calculator.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Tools;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class CancelBonusAccountCommandHandlerTests : CommandHandlerBaseTests<CancelBonusAccountCommandHandler,
    CancelBonusAccountCommand, Unit, Calculator.Domain.Account.Account>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    {
        var parameters = Fixture.Create<CancelBonusAccountParameters>();
        var request = CancelBonusAccountCommand.Create(parameters);
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var message =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(request, CancellationToken.None));

        Assert.Equal(AggregateNullMessageError("Account"), message.Message);
    }

    [Fact]
    public async Task Should_Cancel_Bonus()
    {
        var account = Fixture.GenerateAccountWithBonuses();
        var someBonus = account.Bonuses[0];
        var parameters = Fixture.Build<CancelBonusAccountParameters>()
            .With(w => w.BonusCode, someBonus.BonusCode)
            .Create();
        var request = CancelBonusAccountCommand.Create(parameters);

        account.Details.SetNewValue(nameof(account.Details.IsActive), true);
        var organizationCode = account.Details.CompanyCode;

        CurrentUserMock.SetupGet(_ => _.OrganizationCode).Returns(organizationCode);
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);

        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Calculator.Domain.Account.Account>()), Times.Once);
    }
}