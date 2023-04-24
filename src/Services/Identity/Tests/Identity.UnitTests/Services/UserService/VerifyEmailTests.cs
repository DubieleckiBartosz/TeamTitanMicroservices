using AutoFixture;
using Identity.Application.Models.DataTransferObjects;
using Identity.UnitTests.Generators;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class VerifyEmailTests : UserServiceBaseTests
{ 
    [Fact]
    public async Task Should_Verify_Account_Successfully()
    {
        //Arrange
        var someToken = Fixture.Create<string>();
        var verifyAccountDto = new VerifyAccountDto(someToken);
        var user = Fixture.GenerateUser();
        var confirmationBefore = user.IsConfirmed;

        UserRepositoryMock.Setup(_ => _.FindUserByVerificationTokenAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act
        var result = await UserService.VerifyEmail(verifyAccountDto);

        //Assert
        var confirmationAfter = user.IsConfirmed;

        Assert.True(result.Success);
        Assert.False(confirmationBefore);
        Assert.True(confirmationAfter);
    }
}