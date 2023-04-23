using AutoFixture;
using Calculator.Application.Features.Account.Commands.InitiationAccount;
using MediatR;
using Moq;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class InitiationAccountHandlerTests : CommandHandlerBaseTests<InitiationAccountCommandHandler,
    InitiationAccountCommand, Unit, Calculator.Domain.Account.Account>
{
    [Fact]
    public async Task Should_Publish_Message_When_Account_Initiated()
    {
        var companyCode = Fixture.Create<string>();
        var accountOwnerCode = Fixture.Create<string>();
        var creator = Fixture.Create<string>();

        var request = InitiationAccountCommand.Create(companyCode, accountOwnerCode, creator);

        await Handler.Handle(request, CancellationToken.None);

        AggregateRepositoryMock.Verify(v => 
                v.AddAndPublishAsync(It.IsAny<Calculator.Domain.Account.Account>()), Times.Once);
    }
}