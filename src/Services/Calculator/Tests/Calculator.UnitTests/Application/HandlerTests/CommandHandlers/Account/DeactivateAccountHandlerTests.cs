using AutoFixture;
using Calculator.Application.Features.Account.Commands.DeactivateAccount;
using Calculator.Application.Parameters.AccountParameters;
using Calculator.Domain.Account.Snapshots;
using Calculator.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class DeactivateAccountHandlerTests : CommandHandlerBaseTests<DeactivateAccountCommandHandler,
    DeactivateAccountCommand, Unit, Calculator.Domain.Account.Account>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    {
        var parameters = Fixture.Create<DeactivateAccountParameters>();
        var request = DeactivateAccountCommand.Create(parameters);
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var message =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(request, CancellationToken.None));

        Assert.Equal(AggregateNullMessageError("Account"), message.Message);
    }

    [Fact]
    public async Task Should_Update_Financial_Data()
    {
        var account = Fixture.GenerateAccountWithBaseData();
        var organizationCode = account.Details.CompanyCode;
        var parameters = Fixture.Build<DeactivateAccountParameters>().With(w => w.AccountId, account.Id).Create();
        var request = DeactivateAccountCommand.Create(parameters);

        CurrentUserMock.SetupGet(_ => _.OrganizationCode).Returns(organizationCode);
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);
        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Calculator.Domain.Account.Account>()), Times.Once);
    }
}