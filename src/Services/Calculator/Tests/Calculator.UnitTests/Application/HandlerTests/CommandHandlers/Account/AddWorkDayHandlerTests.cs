using AutoFixture;
using Calculator.Application.Features.Account.Commands.AddWorkDay;
using Calculator.Application.Parameters.AccountParameters;
using Calculator.Domain.Account.Snapshots;
using Calculator.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Tools;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class
    AddWorkDayHandlerTests : CommandHandlerBaseTests<AddWorkDayHandler, AddWorkDayCommand, Unit, Domain.Account.Account>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    {
        var parameters = Fixture.Create<AddWorkDayParameters>();
        var request = AddWorkDayCommand.Create(parameters);
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var message =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(request, CancellationToken.None));

        Assert.Equal(AggregateNullMessageError("Account"), message.Message);
    }

    [Fact]
    public async Task Should_Add_New_WorkDay()
    {
        var account = Fixture.GenerateAccountWithBaseData();

        var parameters = Fixture.Build<AddWorkDayParameters>()
            .With(w => w.IsDayOff, true)
            .With(w => w.HoursWorked, account.Details.WorkDayHours - 1).Create();
        var request = AddWorkDayCommand.Create(parameters);

        account.Details.SetNewValue(nameof(account.Details.IsActive), true);
        var organizationCode = account.Details.CompanyCode;

        CurrentUserMock.SetupGet(_ => _.OrganizationCode).Returns(organizationCode);
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);

        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Domain.Account.Account>()), Times.Once);
    }
}