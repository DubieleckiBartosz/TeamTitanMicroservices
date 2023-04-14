using Management.Application.Constants;
using Management.Application.Features.Commands.Contract.AddContract;
using Management.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Contract;

public class AddContractHandlerTests : CommandHandlerBaseTests<NewContractHandler, NewContractCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_Employee_Not_Found()
    {
        var command = Fixture.GetNewContractCommand();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeWithContractsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var responseException =
            await Assert.ThrowsAsync<NotFoundException>(() => Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Employee"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetEmployeeWithContractsByIdAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_CompleteAsync_Method_When_Contract_Added_To_Employee()
    {
        var command = Fixture.GetNewContractCommand();
        var employee = Fixture.GetEmployeeDaoWithContracts();

        EmployeeRepositoryMock.Setup(_ => _.GetEmployeeWithContractsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => employee);

        await Handler.Handle(command, CancellationToken.None);

        UnitOfWork.Verify(v => v.CompleteAsync(It.IsAny<Domain.Entities.Employee>()), Times.Once);
    }
}