using Management.Application.Constants;
using Management.Application.Features.Commands.Contract.UpdateBankAccount;
using Management.UnitTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Contract;

public class UpdateBankAccountHandlerTests : CommandHandlerBaseTests<UpdateBankAccountHandler, UpdateBankAccountCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Contract_Not_Found()
    {
        var command = Fixture.GetUpdateBankAccountCommand();

        ContractRepositoryMock.Setup(_ => _.GetContractByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Contract"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetContractByIdAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_UpdateBankAccountNumberAsync_Method_When_BankAccountNumber_Updated_Or_Added()
    {
        var command = Fixture.GetUpdateBankAccountCommand();
        var contract = Fixture.GetContractDao();

        ContractRepositoryMock.Setup(_ => _.GetContractByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => contract);

        await Handler.Handle(command, CancellationToken.None);

        ContractRepositoryMock.Verify(v => v.UpdateBankAccountNumberAsync(It.IsAny<Management.Domain.Entities.Contract>()), Times.Once);
    }
}