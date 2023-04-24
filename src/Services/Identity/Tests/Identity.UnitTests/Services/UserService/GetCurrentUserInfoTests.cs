using AutoFixture;
using Identity.Domain.Entities;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class GetCurrentUserInfoTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Returns_User_Info()
    {
        //Arrange
        var token = Fixture.Create<string>();
        var user = Fixture.Create<User>();

        UserRepositoryMock.Setup(_ => _.FindUserByTokenAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act
        var result = await UserService.GetCurrentUserInfoAsync(token);

        //Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }
}