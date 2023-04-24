using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Moq;
using System.Net;
using Identity.UnitTests.Generators;
using Shared.Implementations.Tools;

namespace Identity.UnitTests.Services.UserService;

public class ResetPasswordTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_IdentityResultException_When_User_Not_Found()
    {
        //Arrange
        var parameters = Fixture.Create<ResetPasswordParameters>();
        var resetPasswordDto = new ResetPasswordDto(parameters);

        UserRepositoryMock.Setup(_ => _.FindUserByResetTokenAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        //Act 
        var resultException =
            await Assert.ThrowsAsync<IdentityResultException>(() =>
                UserService.ResetPasswordAsync(resetPasswordDto));

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, resultException.StatusCode);
        Assert.Equal(ExceptionIdentityMessages.UserNotFound, resultException.Message);
    }

    [Fact]
    public async Task Should_Throw_IdentityResultException_When_ResetToken_Is_Null()
    {
        //Arrange
        var parameters = Fixture.Create<ResetPasswordParameters>();
        var resetPasswordDto = new ResetPasswordDto(parameters);
        var user = Fixture.GenerateUser();

        UserRepositoryMock.Setup(_ => _.FindUserByResetTokenAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act 
        var resultException =
            await Assert.ThrowsAsync<IdentityResultException>(() =>
                UserService.ResetPasswordAsync(resetPasswordDto));

        //Assert
        Assert.Equal(HttpStatusCode.InternalServerError, resultException.StatusCode);
        Assert.Equal(ExceptionIdentityMessages.ResetTokenExpired, resultException.Message);
    }

    [Fact]
    public async Task Should_Throw_IdentityResultException_When_ResetToken_Is_Not_Active()
    {
        //Arrange
        var parameters = Fixture.Create<ResetPasswordParameters>();
        var resetPasswordDto = new ResetPasswordDto(parameters);
        var user = Fixture.GenerateApprovedUserWithResetToken();

        user.ResetToken.SetNewValue(nameof(user.ResetToken.TokenExpirationDate), DateTime.UtcNow.AddMonths(-1));

        UserRepositoryMock.Setup(_ => _.FindUserByResetTokenAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act 
        var resultException =
            await Assert.ThrowsAsync<IdentityResultException>(() =>
                UserService.ResetPasswordAsync(resetPasswordDto));

        //Assert
        Assert.Equal(HttpStatusCode.InternalServerError, resultException.StatusCode);
        Assert.Equal(ExceptionIdentityMessages.ResetTokenExpired, resultException.Message);
    }

    [Fact]
    public async Task Should_Change_Password_Successfully()
    {
        //Arrange
        var parameters = Fixture.Create<ResetPasswordParameters>();
        var resetPasswordDto = new ResetPasswordDto(parameters);
        var user = Fixture.GenerateApprovedUserWithResetToken(); 

        UserRepositoryMock.Setup(_ => _.FindUserByResetTokenAsync(It.IsAny<string>())).ReturnsAsync(user);

        var passwordBefore = user.PasswordHash;

        //Act 
        var result = await UserService.ResetPasswordAsync(resetPasswordDto);

        //Assert 
        var passwordAfter = user.PasswordHash;

        Assert.True(result.Success);
        Assert.NotEqual(passwordBefore, passwordAfter);
    } 
}