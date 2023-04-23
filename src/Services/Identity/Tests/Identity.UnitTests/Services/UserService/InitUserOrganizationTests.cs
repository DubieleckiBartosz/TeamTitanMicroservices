using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Application.Enums;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class InitUserOrganizationTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_Dto_Is_null()
    {
        //Act and assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => UserService.InitUserOrganizationAsync(null));
    }

    [Fact]
    public async Task Should_Throw_AuthException_When_Code_Is_In_Use()
    {
        //Arrange
        var parameters = Fixture.Create<InitUserOrganizationParameters>();
        var initUserOrganizationDto = new InitUserOrganizationDto(parameters);

        UserRepositoryMock.Setup(_ => _.CodeExistsAsync(It.IsAny<string>())).ReturnsAsync(() => true);

        //Act and assert
        var resultException =
            await Assert.ThrowsAsync<AuthException>(
                () => UserService.InitUserOrganizationAsync(initUserOrganizationDto));

        Assert.Equal(ExceptionIdentityTitles.IncorrectCode, resultException.Title);
    }

    [Fact]
    public async Task Should_Returns_Success_Response_And_Should_Send_Mail()
    {
        //Arrange
        var parameters = Fixture.Build<InitUserOrganizationParameters>()
            .With(w => w.Role, Roles.Manager.ToString())
            .Create();

        var initUserOrganizationDto = new InitUserOrganizationDto(parameters);

        UserRepositoryMock.Setup(_ => _.CodeExistsAsync(It.IsAny<string>())).ReturnsAsync(() => false);

        //Act 
        var result = await UserService.InitUserOrganizationAsync(initUserOrganizationDto);

        //Assert
        Assert.True(result.Success);
        IdentityEmailServiceMock.Verify(v =>
            v.SendEmailInitUserOrganizationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    } 
}