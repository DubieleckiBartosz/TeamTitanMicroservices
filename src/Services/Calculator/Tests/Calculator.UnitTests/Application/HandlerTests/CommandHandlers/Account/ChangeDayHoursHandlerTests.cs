using Calculator.Application.Features.Account.Commands.ChangeDayHours;
using Calculator.Domain.Account.Snapshots;
using Calculator.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ChangeDayHoursHandlerTests : CommandHandlerBaseTests<ChangeDayHoursCommandHandler, ChangeDayHoursCommand,
    Unit, Calculator.Domain.Account.Account>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    { 
        var request = ChangeDayHoursCommand.Create(this.GetRandomInt(), Guid.NewGuid());
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var message =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(request, CancellationToken.None));

        Assert.Equal(AggregateNullMessageError("Account"), message.Message);
    }

    [Fact]
    public async Task Should_Change_Day_Hours()
    {
        var account = Fixture.GenerateAccountWithBaseData(); 
        var organizationCode = account.Details.CompanyCode;
        var request = ChangeDayHoursCommand.Create(this.GetRandomInt(), account.Id);

        CurrentUserMock.SetupGet(_ => _.OrganizationCode).Returns(organizationCode);
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);
        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Calculator.Domain.Account.Account>()), Times.Once);
    } 
}