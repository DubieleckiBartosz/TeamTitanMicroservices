using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Identity.Domain.Entities;
using Identity.UnitTests.Generators;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class RegisterUserTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_Dto_Is_null()
    {
        //Act and assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => UserService.RegisterNewUserAsync(null as RegisterDto, It.IsAny<string>()));
    }

    [Fact]
    public async Task Should_Throw_AuthException_When_User_Exists()
    {
        //Arrange
        var parameters = Fixture.Create<RegisterParameters>();
        var registerDto = new RegisterDto(parameters);
        var user = Fixture.GenerateUser();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act and assert
        var resultException = await Assert.ThrowsAsync<AuthException>(() => UserService.RegisterNewUserAsync(registerDto, It.IsAny<string>()));

        Assert.Equal(ExceptionIdentityTitles.UserExists, resultException.Title);
    }

    [Fact]
    public async Task Should_Call_logError_Method_When_Creating_User_Failed()
    {
        //Arrange
        var messageSomeError = Fixture.Create<string>();
        var parameters = Fixture.Create<RegisterParameters>();
        var registerDto = new RegisterDto(parameters);
        var hashPwd = Fixture.Create<string>();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);
        PasswordHasherMock.Setup(_ => _.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(hashPwd);
        UserRepositoryMock.Setup(_ => _.CreateAsync(It.IsAny<User>())).ThrowsAsync(new Exception(messageSomeError));

        //Act and assert
        var resultException =
            await Assert.ThrowsAsync<Exception>(() =>
                UserService.RegisterNewUserAsync(registerDto, It.IsAny<string>()));

        Assert.Equal(messageSomeError, resultException.Message);
        LoggerManagerMock.Verify(v => v.LogError(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Should_Returns_Success_And_Should_Send_Mail()
    {
        //Arrange 
        var parameters = Fixture.Create<RegisterParameters>();
        var registerDto = new RegisterDto(parameters); 
        var hashPwd = Fixture.Create<string>();
        var identifier = Fixture.Create<int>();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);
        PasswordHasherMock.Setup(_ => _.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(hashPwd);
        UserRepositoryMock.Setup(_ => _.CreateAsync(It.IsAny<User>())).ReturnsAsync(identifier);

        //Act 
        var result = await UserService.RegisterNewUserAsync(registerDto, TestOrigin);

        //Assert
        Assert.True(result.Success);
        IdentityEmailServiceMock.Verify(
            v => v.SendEmailAfterCreateNewAccountAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }
}