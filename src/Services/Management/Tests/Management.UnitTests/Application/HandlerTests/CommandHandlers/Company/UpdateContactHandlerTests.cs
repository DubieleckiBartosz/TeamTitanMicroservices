using Management.Application.Constants;
using Management.Application.Features.Commands.Company.UpdateContact;
using Management.UnitTests.Application.HandlerTests.ModelGenerators;
using MediatR;
using Moq;
using Shared.Implementations.Core.Exceptions;

namespace Management.UnitTests.Application.HandlerTests.CommandHandlers.Company;

public class UpdateContactHandlerTests : CommandHandlerBaseTests<UpdateCompanyContactHandler, UpdateCompanyContactCommand, Unit>
{
    [Fact]
    public async Task Should_Throw_NotFoundException_When_GetCompanyByOwnerCodeAsync_Method_Returns_Null()
    {
        var command = Fixture.GetUpdateCompanyContactCommand();

         CompanyRepositoryMock.Setup(_ => _.GetCompanyByCodeAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        var responseException = await Assert.ThrowsAsync<NotFoundException>(() => this.Handler.Handle(command, CancellationToken.None));

        Assert.Equal(Messages.DataNotFoundMessage("Company"), responseException.Message);
        Assert.Equal(Titles.MethodFailedTitle("GetCompanyByCodeAsync"), responseException.Title);
    }

    [Fact]
    public async Task Should_Call_UpdateCommunicationDataAsync_Method_When_Company_Contact_Updated()
    {
        var command = Fixture.GetUpdateCompanyContactCommand();
        var companyDao = Fixture.GetCompanyDao();
         
        CompanyRepositoryMock.Setup(_ => _.GetCompanyByCodeAsync(It.IsAny<string>())).ReturnsAsync(() => companyDao);

        await this.Handler.Handle(command, CancellationToken.None);

        this.CompanyRepositoryMock.Verify(v => v.UpdateCommunicationDataAsync(It.IsAny<Domain.Entities.Company>()), Times.Once);
    }
}