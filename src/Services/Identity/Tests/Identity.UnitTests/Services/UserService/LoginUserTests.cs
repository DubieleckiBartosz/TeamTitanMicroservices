using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Identity.Domain.Entities;
using Identity.UnitTests.Generators;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class LoginUserTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_Dto_Is_null()
    {
        //Act and assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => UserService.LoginAsync(null as LoginDto));
    }

    [Fact]
    public async Task Should_Throw_IdentityResultException_When_User_Not_Found()
    {
        //Arrange
        var parameters = Fixture.Create<LoginParameters>();
        var loginDto = new LoginDto(parameters);

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        //Act and assert
        var resultException = await Assert.ThrowsAsync<IdentityResultException>(() => UserService.LoginAsync(loginDto));
         
        Assert.Equal(ExceptionIdentityTitles.UserByEmail, resultException.Title);
    }

    [Fact]
    public async Task Should_Throw_IdentityResultException_When_User_Is_Not_Approved()
    {
        //Arrange
        var parameters = Fixture.Create<LoginParameters>();
        var loginDto = new LoginDto(parameters);
        var user = Fixture.GenerateUser();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act and assert
        var resultException = await Assert.ThrowsAsync<IdentityResultException>(() => UserService.LoginAsync(loginDto));

        Assert.Equal(ExceptionIdentityMessages.AccountNotApproval, resultException.Message);
    }

    [Fact]
    public async Task Should_Throw_AuthException_When_PasswordVerificationResult_Equals_Failed()
    {
        //Arrange
        var parameters = Fixture.Create<LoginParameters>();
        var loginDto = new LoginDto(parameters);
        var user = Fixture.GenerateApprovedUser();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        PasswordHasherMock.Setup(_ => _.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => PasswordVerificationResult.Failed);

        //Act and assert
        var resultException = await Assert.ThrowsAsync<AuthException>(() => UserService.LoginAsync(loginDto));

        Assert.Equal(ExceptionIdentityMessages.MethodException("VerifyHashedPassword"), resultException.Message);
    }


    [Fact]
    public async Task Should_Return_Success_And_Should_Call_UpdateAsync_Method_When_RefreshToken_Is_Not_Active()
    {
        //Arrange
        var parameters = Fixture.Create<LoginParameters>();
        var loginDto = new LoginDto(parameters);
        var user = Fixture.GenerateApprovedUser();
        var newRefreshToken = Fixture.Create<string>();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        PasswordHasherMock.Setup(_ => _.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => PasswordVerificationResult.Success);
        PasswordHasherMock.Setup(_ => _.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(newRefreshToken);

        //Act 
        var result = await UserService.LoginAsync(loginDto);

        //Assert
        Assert.True(result.Success);
        UserRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Success_And_Should_Never_Call_UpdateAsync_Method_When_RefreshToken_Is_Active()
    {
        //Arrange
        var parameters = Fixture.Create<LoginParameters>();
        var loginDto = new LoginDto(parameters);
        var user = Fixture.GenerateApprovedUserWithActiveRefreshToken();
        var newRefreshToken = Fixture.Create<string>();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        PasswordHasherMock.Setup(_ => _.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => PasswordVerificationResult.Success);
        PasswordHasherMock.Setup(_ => _.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(newRefreshToken);

        //Act 
        var result = await UserService.LoginAsync(loginDto);

        //Assert
        Assert.True(result.Success);
        UserRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<User>()), Times.Never);
    }
}