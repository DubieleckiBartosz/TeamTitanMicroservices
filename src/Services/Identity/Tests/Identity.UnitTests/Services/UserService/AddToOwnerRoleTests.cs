using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Identity.Domain.Entities;
using Identity.UnitTests.Generators;
using Moq;
using Shared.Implementations.Tools;

namespace Identity.UnitTests.Services.UserService;

public class AddToOwnerRoleTests : UserServiceBaseTests
{
    private readonly string _organizationCode;
    private readonly string _recipient;
    private readonly string _ownerCode;

    public AddToOwnerRoleTests()
    {
        _organizationCode = Fixture.Create<string>().Encrypt(EncryptionSettings.OwnerRoleEncryptionKey);
        _recipient = Fixture.Create<string>().Encrypt(EncryptionSettings.OwnerRoleEncryptionKey);
        _ownerCode = Fixture.Create<string>().Encrypt(EncryptionSettings.OwnerRoleEncryptionKey);
    }

    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_Dto_Is_null()
    {
        //Act and assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => UserService.AddToOwnerRoleAsync(null as UserOwnerRoleDto));
    }

    [Fact]
    public async Task Should_Throw_IdentityResultException_When_User_Not_Found()
    {
        //Arrange  
        var parameters = Fixture.Build<UserOwnerRoleParameters>()
            .With(w => w.OwnerCode, _ownerCode)
            .With(w => w.Recipient, _recipient)
            .With(w => w.Organization, _organizationCode).Create();
   
        var userOwnerRoleDto = new UserOwnerRoleDto(parameters);

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        //Act and assert
        var resultException = await Assert.ThrowsAsync<IdentityResultException>(() => UserService.AddToOwnerRoleAsync(userOwnerRoleDto));

        Assert.Equal(ExceptionIdentityTitles.UserByEmail, resultException.Title);
    }

    [Fact]
    public async Task Should_Mark_User_As_Owner()
    {
        //Arrange  
        var parameters = Fixture.Build<UserOwnerRoleParameters>()
            .With(w => w.OwnerCode, _ownerCode)
            .With(w => w.Recipient, _recipient)
            .With(w => w.Organization, _organizationCode).Create();

        var userOwnerRoleDto = new UserOwnerRoleDto(parameters);
        var user = Fixture.GenerateApprovedUser();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        var beforeRoles = user.Roles.Count; 

        //Act 
        var result = await UserService.AddToOwnerRoleAsync(userOwnerRoleDto);

        //Assert
        var afterRoles = user.Roles.Count;

        Assert.True(result.Success);
        Assert.Equal(beforeRoles + 1, afterRoles);
        UserRepositoryMock.Verify(v=>v.AddToOwnerAsync(It.IsAny<User>()), Times.Once);
    }
}