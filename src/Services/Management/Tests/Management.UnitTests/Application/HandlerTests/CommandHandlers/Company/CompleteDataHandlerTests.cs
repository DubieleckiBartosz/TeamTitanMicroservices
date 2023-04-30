using Management.Application.Constants;
using Management.Application.Features.Commands.Company.CompleteData;
using Management.UnitTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Company;

public class CompleteDataHandlerTests : CommandHandlerBaseTests<CompleteDataHandler, CompleteDataCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_BadRequestException_When_Company_Name_Exists()
    {
        //Arrange
        var command = Fixture.GetCompleteDataCommand();

        CompanyRepositoryMock.Setup(_ => _.CompanyNameExistsAsync(It.IsAny<string>())).ReturnsAsync(() => true);

        //Act and assert
        var responseException = await Assert.ThrowsAsync<BadRequestException>(() => this.Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.NameExistsMessageException, responseException.Message);
        Assert.Equal(Titles.NameExistsTitle, responseException.Title);
    }

    [Fact]
    public async Task Should_Throw_NotFoundException_When_GetCompanyByOwnerCodeAsync_Method_Returns_Null()
    {
        //Arrange
        var command = Fixture.GetCompleteDataCommand();

        CompanyRepositoryMock.Setup(_ => _.CompanyNameExistsAsync(It.IsAny<string>())).ReturnsAsync(() => false);
        CompanyRepositoryMock.Setup(_ => _.GetCompanyByOwnerCodeAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        //Act and assert
        var responseException = await Assert.ThrowsAsync<NotFoundException>(() => this.Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Company"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetCompanyByOwnerCodeAsync"), responseException.Title);
    }


    [Fact]
    public async Task Should_Call_CompleteDataAsync_Method_When_Company_Data_Completed()
    {
        //Arrange
        var command = Fixture.GetCompleteDataCommand();
        var companyDao = Fixture.GetCompanyDao();

        CompanyRepositoryMock.Setup(_ => _.CompanyNameExistsAsync(It.IsAny<string>())).ReturnsAsync(() => false);
        CompanyRepositoryMock.Setup(_ => _.GetCompanyByOwnerCodeAsync(It.IsAny<string>())).ReturnsAsync(() => companyDao);

        //Act
        await this.Handler.Handle(command, CancellationToken.None);
         
        //Assert
        this.CompanyRepositoryMock.Verify(v=>v.CompleteDataAsync(It.IsAny<Management.Domain.Entities.Company>()), Times.Once);
    }
}