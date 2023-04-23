using AutoFixture;
using Identity.Application.Contracts.Services;
using Identity.Application.Contracts;
using Identity.Domain.Entities;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using Shared.Implementations.Logging;
using Shared.Implementations.Services;

namespace Identity.UnitTests.Services.UserService;

public abstract class UserServiceBaseTests
{
    protected readonly Mock<IUserRepository> UserRepositoryMock;
    protected readonly Mock<IPasswordHasher<User>> PasswordHasherMock;
    protected readonly Mock<ILoggerManager<Application.Services.UserService>> LoggerManagerMock;
    protected readonly Mock<IIdentityEmailService> IdentityEmailServiceMock;
    protected readonly Mock<ICurrentUser> CurrentUserMock;
    protected readonly Mock<IOptions<JwtSettings>> JwtSettingsOptions; 
    protected readonly JwtSettings JwtSettings;
    protected Application.Services.UserService UserService;

    protected Fixture Fixture;
    protected AutoMocker Mocker;
 
    protected UserServiceBaseTests()
    {
        this.Fixture = new Fixture();
        this.Mocker = new AutoMocker();
        this.UserRepositoryMock = Mocker.GetMock<IUserRepository>();
        this.PasswordHasherMock = Mocker.GetMock<IPasswordHasher<User>>();
        this.LoggerManagerMock = Mocker.GetMock<ILoggerManager<Application.Services.UserService>>();
        this.IdentityEmailServiceMock = Mocker.GetMock<IIdentityEmailService>();
        this.CurrentUserMock = Mocker.GetMock<ICurrentUser>();
        this.JwtSettings = Fixture.Build<JwtSettings>().Create();
        this.JwtSettingsOptions = Mocker.GetMock<IOptions<JwtSettings>>();
        this.JwtSettingsOptions.Setup(_ => _.Value).Returns(JwtSettings);
        this.UserService = Mocker.CreateInstance<Application.Services.UserService>();
    } 
}