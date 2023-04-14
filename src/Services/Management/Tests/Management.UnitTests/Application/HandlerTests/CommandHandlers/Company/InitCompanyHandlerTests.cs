using Management.Application.Features.Commands.Company.InitCompany;
using Management.UnitTests.ModelGenerators;
using MediatR;
using Moq;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Company;

public class InitCompanyHandlerTests : CommandHandlerBaseTests<InitCompanyHandler, InitCompanyCommand, Unit>
{
    [Fact]
    public async Task Should_Call_CompanyCodeExistsAsync_Twice_When_Code_Generated_First_Time_Already_Exists()
    {
        var command = this.Fixture.GetInitCompanyCommand();

        CompanyRepositoryMock.SetupSequence(_ => _.CompanyCodeExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(() => true)
            .ReturnsAsync(() => false);

        await Handler.Handle(command, CancellationToken.None);
        CompanyRepositoryMock.Verify(v=>v.CompanyCodeExistsAsync(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Should_Call_CompleteAsync_When_Company_Initiated()
    {
        var command = this.Fixture.GetInitCompanyCommand();

        CompanyRepositoryMock.SetupSequence(_ => _.CompanyCodeExistsAsync(It.IsAny<string>())) 
            .ReturnsAsync(() => false);

        await Handler.Handle(command, CancellationToken.None);

        UnitOfWork.Verify(v => v.CompleteAsync(It.IsAny<Management.Domain.Entities.Company>()), Times.Once);
    }
}