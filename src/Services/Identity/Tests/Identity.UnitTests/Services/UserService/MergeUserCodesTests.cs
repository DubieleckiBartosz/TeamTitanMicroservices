using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Identity.UnitTests.Generators;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class MergeUserCodesTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_Dto_Is_null()
    { 
        await Assert.ThrowsAsync<ArgumentNullException>(() => UserService.MergeUserCodesAsync(null as AssignUserCodesDto));
    }

    [Fact]
    public async Task Should_Throw_AuthException_When_Code_Is_In_Use()
    {
        //Arrange
        var parameters = Fixture.Create<AssignUserCodesParameters>();
        var assignUserCodesDto = new AssignUserCodesDto(parameters);

        UserRepositoryMock.Setup(_ => _.CodeIsInUseAsync(It.IsAny<string>())).ReturnsAsync(() => true);

        //Act and assert
        var resultException = await Assert.ThrowsAsync<AuthException>(() => UserService.MergeUserCodesAsync(assignUserCodesDto));

        Assert.Equal(ExceptionIdentityTitles.IncorrectCode, resultException.Title);
    }

    [Fact]
    public async Task Should_Returns_Success_When_Codes_Merged()
    {
        //Arrange
        var parameters = Fixture.Create<AssignUserCodesParameters>();
        var assignUserCodesDto = new AssignUserCodesDto(parameters);
        var userId = Fixture.Create<int>();
        var userResponseRepository = Fixture.GenerateUser();

        UserRepositoryMock.Setup(_ => _.CodeIsInUseAsync(It.IsAny<string>())).ReturnsAsync(() => false);
        CurrentUserMock.SetupGet(_ => _.UserId).Returns(userId);
        UserRepositoryMock.Setup(_ => _.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(userResponseRepository);
        //Act
        var result = await UserService.MergeUserCodesAsync(assignUserCodesDto);

        //Assert 
        Assert.True(result.Success);
    }
}