using AutoFixture;
using Calculator.Application.Features.Account.Commands.UpdateData;
using Calculator.Domain.Account.Snapshots;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Calculator.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class UpdateAccountDataHandlerTests : CommandHandlerBaseTests<UpdateAccountDataCommandHandler,
    UpdateAccountDataCommand, Unit, Calculator.Domain.Account.Account>
{
    private readonly CountingType _countingType;
    private readonly AccountStatus _status;
    private readonly int _workDayHours;
    private readonly int _settlementDayMonth;
    private readonly DateTime _expirationDate;

    public UpdateAccountDataHandlerTests()
    {
        _countingType = Fixture.Create<CountingType>();
        _status = Fixture.Create<AccountStatus>();
        _workDayHours = Fixture.Create<int>();
        _settlementDayMonth = GetRandomInt();
        _expirationDate = DateTime.UtcNow.AddMonths(3);
    }

    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    {
        var request = UpdateAccountDataCommand.Create(_countingType, _status, _workDayHours,
            _settlementDayMonth, Guid.NewGuid(), _expirationDate);
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var message =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(request, CancellationToken.None));

        Assert.Equal(
            AggregateNullMessageError("Account"), message.Message);
    }

    [Fact]
    public async Task Should_Update_Base_Data_And_Should_Not_Call_JobService_When_Settlement_Day_Is_The_Same()
    {
        var account = Fixture.GenerateAccountWithBaseData(settlementDayMonth: _settlementDayMonth);

        var accountId = account.Id;
        var request = UpdateAccountDataCommand.Create(_countingType, _status, _workDayHours,
            _settlementDayMonth, accountId, _expirationDate);

        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);

        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Calculator.Domain.Account.Account>()), Times.Once);
        JobServiceMock.Verify(
            v => v.RecurringJobMediator(It.IsAny<string>(), It.IsAny<ICommand<Unit>>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Should_Update_Base_Data_And_Should_Call_JobService_When_Settlement_Days_Are_Different()
    {
        var account = Fixture.GenerateAccountWithBaseData(settlementDayMonth: GetRandomInt());

        var accountId = account.Id;
        var request = UpdateAccountDataCommand.Create(_countingType, _status, _workDayHours,
            GetRandomInt(11, 20), accountId, _expirationDate);

        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);

        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Calculator.Domain.Account.Account>()), Times.Once);
        JobServiceMock.Verify(
            v => v.RecurringJobMediator(It.IsAny<string>(), It.IsAny<ICommand<Unit>>(), It.IsAny<string>()),
            Times.Once);
    }
}