using AutoFixture;
using Identity.Application.Contracts.Services;
using Identity.Application.Contracts;
using Identity.Application.Settings;
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
    protected const string TestOrigin = "https://0.0.0.0";

    protected readonly Mock<IUserRepository> UserRepositoryMock;
    protected readonly Mock<IPasswordHasher<User>> PasswordHasherMock;
    protected readonly Mock<ILoggerManager<Application.Services.UserService>> LoggerManagerMock;
    protected readonly Mock<IIdentityEmailService> IdentityEmailServiceMock;
    protected readonly Mock<ICurrentUser> CurrentUserMock;
    protected readonly Mock<IOptions<JwtSettings>> JwtSettingsOptionsMock; 
    protected readonly Mock<IOptions<EncryptionSettings>> EncryptionSettingsOptionsMock; 
    protected readonly JwtSettings JwtSettings;
    protected readonly EncryptionSettings EncryptionSettings;
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
        this.JwtSettingsOptionsMock = Mocker.GetMock<IOptions<JwtSettings>>();
        this.JwtSettingsOptionsMock.Setup(_ => _.Value).Returns(JwtSettings);
        this.EncryptionSettings = Fixture.Build<EncryptionSettings>()
            .With(w => w.OwnerRoleEncryptionKey, "b14ca5898a4e4133bbce2ea2315a1916").Create();
        this.EncryptionSettingsOptionsMock = Mocker.GetMock<IOptions<EncryptionSettings>>();
        this.EncryptionSettingsOptionsMock.Setup(_ => _.Value).Returns(EncryptionSettings);
        this.UserService = Mocker.CreateInstance<Application.Services.UserService>();
    }
}