using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Domain.Entities;
using Identity.UnitTests.Generators;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class RefreshTokenTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_AuthException_When_RefreshToken_Is_Null_Or_Empty()
    {
        //Act and assert
        await Assert.ThrowsAsync<AuthException>(() => UserService.RefreshTokenAsync(string.Empty));
    }

    [Fact]
    public async Task Should_Throw_AuthException_When_RefreshToken_Not_Found()
    {
        //Arrange
        var someToken = Fixture.Create<string>();
        var user = Fixture.GenerateUser();

        UserRepositoryMock.Setup(_ => _.FindUserByTokenAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act and assert
        var result = await Assert.ThrowsAsync<AuthException>(() =>
            UserService.RefreshTokenAsync(someToken));

        Assert.Equal(ExceptionIdentityTitles.ValidationError, result.Title);
    }

    [Fact]
    public async Task Should_Refresh_Token()
    {
        //Arrange
        var user = Fixture.GenerateApprovedUserWithActiveRefreshToken();
        var activeToken = user.RefreshTokens[0].TokenValue.Token;
        var newToken = Fixture.Create<string>();

        UserRepositoryMock.Setup(_ => _.FindUserByTokenAsync(It.IsAny<string>())).ReturnsAsync(user);
        PasswordHasherMock.Setup(_ => _.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(newToken);
        //Act
        var result = await UserService.RefreshTokenAsync(activeToken);

        //Assert
        Assert.True(result.Success);
    }
}