using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Application.Enums;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Identity.UnitTests.Generators;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class AddToRoleTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_Dto_Is_null()
    {
        //Act and assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => UserService.AddToRoleAsync(null as UserNewRoleDto));
    }

    [Fact]
    public async Task Should_Throw_IdentityResultException_When_User_Not_Found()
    {
        //Arrange 
        var parameters = Fixture.Create<UserNewRoleParameters>();
        var userNewRoleDto = new UserNewRoleDto(parameters);

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        //Act and assert
        var resultException = await Assert.ThrowsAsync<IdentityResultException>(() => UserService.AddToRoleAsync(userNewRoleDto));

        Assert.Equal(ExceptionIdentityTitles.UserByEmail, resultException.Title);
    }

    [Fact]
    public async Task Should_Throw_IdentityResultException_When_User_Is_Not_Approved()
    {
        //Arrange
        var parameters = Fixture.Create<UserNewRoleParameters>();
        var userNewRoleDto = new UserNewRoleDto(parameters);
        var user = Fixture.GenerateUser();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act and assert
        var resultException = await Assert.ThrowsAsync<IdentityResultException>(() => UserService.AddToRoleAsync(userNewRoleDto));

        Assert.Equal(ExceptionIdentityMessages.AccountNotApproval, resultException.Message);
    }

    [Fact]
    public async Task Should_Add_User_To_New_Role()
    {
        //Arrange
        var parameters = Fixture.Build<UserNewRoleParameters>()
            .With(w => w.Role, Roles.Admin.ToString())
            .Create();
        var userNewRoleDto = new UserNewRoleDto(parameters);
        var user = Fixture.GenerateApprovedUser();

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user); 

        var rolesBefore = user.Roles.Count;

        //Act
        var result = await UserService.AddToRoleAsync(userNewRoleDto);
        var rolesAfter = user.Roles.Count;

        //Assert
        Assert.True(result.Success);
        Assert.Equal(rolesBefore + 1, rolesAfter);
    }
}