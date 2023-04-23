using AutoFixture;
using Calculator.Application.Features.Account.Commands.ChangeFinancialData;
using Calculator.Domain.Account.Snapshots;
using Calculator.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ChangeFinancialDataCommandHandlerTests : CommandHandlerBaseTests<ChangeFinancialDataCommandHandler,
    ChangeFinancialDataCommand, Unit, Calculator.Domain.Account.Account>
{
    private readonly decimal? _payoutAmount;
    private readonly decimal? _overtimeRate;
    private readonly decimal? _hourlyRate;
    public ChangeFinancialDataCommandHandlerTests()
    {
        _payoutAmount = Fixture.Create<decimal>();
        _overtimeRate = Fixture.Create<decimal>();
        _hourlyRate = Fixture.Create<decimal>();
    }

    [Fact]
    public async Task Should_Throw_NotFoundException_When_Account_Not_Found()
    {
        var request = ChangeFinancialDataCommand.Create(_payoutAmount, _overtimeRate, _hourlyRate, Guid.NewGuid());
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
        var request = ChangeFinancialDataCommand.Create(_payoutAmount, _overtimeRate, _hourlyRate, account.Id);
         
        AggregateRepositoryMock.Setup(_ => _.GetAggregateFromSnapshotAsync<AccountSnapshot>(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        await Handler.Handle(request, CancellationToken.None);
        AggregateRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<Calculator.Domain.Account.Account>()), Times.Once);
    }
}