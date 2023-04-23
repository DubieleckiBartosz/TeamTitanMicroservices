using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.UnitTests.Generators;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class ClearUserCodesTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_AuthException_When_Role_Not_Found()
    {
        //Arrange
        CurrentUserMock.Setup(_ => _.IsInRoles(It.IsAny<string[]>())).Returns(() => false); 

        //Act and assert
        var resultException = await Assert.ThrowsAsync<AuthException>(() => UserService.ClearUserCodesAsync());

        Assert.Equal(ExceptionIdentityTitles.IncorrectRole, resultException.Title);
    }

    [Fact]
    public async Task Should_Clear_Codes()
    {
        //Arrange
        var userId = Fixture.Create<int>();
        var user = Fixture.GenerateUserCompany();

        CurrentUserMock.Setup(_ => _.IsInRoles(It.IsAny<string[]>())).Returns(() => true);
        CurrentUserMock.SetupGet(_ => _.UserId).Returns(userId);
        UserRepositoryMock.Setup(_ => _.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

        //Act
        var response = await UserService.ClearUserCodesAsync();

        //Assert
        Assert.True(response.Success);
    }
}