using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.UnitTests.Generators;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class RevokeTokenTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_BadRequestException_When_Token_Is_Null_Or_Empty()
    {
        //Act and assert
        await Assert.ThrowsAsync<AuthException>(() => UserService.RevokeTokenAsync(string.Empty));
    }

    [Fact]
    public async Task Should_Throw_AuthException_When_Token_Not_Found()
    {
        //Arrange
        var someToken = Fixture.Create<string>();
        var user = Fixture.GenerateUser();

        UserRepositoryMock.Setup(_ => _.FindUserByTokenAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act and assert
        var result = await Assert.ThrowsAsync<AuthException>(() =>
            UserService.RevokeTokenAsync(someToken));

        Assert.Equal(ExceptionIdentityTitles.ValidationError, result.Title);
    }

    [Fact]
    public async Task Should_Revoke_Active_Token()
    {
        //Arrange
        var user = Fixture.GenerateApprovedUserWithActiveRefreshToken();
        var activeToken = user.RefreshTokens[0];

        UserRepositoryMock.Setup(_ => _.FindUserByTokenAsync(It.IsAny<string>())).ReturnsAsync(user);
        var tokenActivityBefore = activeToken.TokenActivity;

        //Act
        var result = await UserService.RevokeTokenAsync(activeToken.TokenValue.Token);

        //Arrange
        var tokenActivityAfter = activeToken.TokenActivity;
       
        Assert.True(result.Success);
        Assert.Null(tokenActivityBefore.Revoked);
        Assert.NotNull(tokenActivityAfter.Revoked);
    }
}