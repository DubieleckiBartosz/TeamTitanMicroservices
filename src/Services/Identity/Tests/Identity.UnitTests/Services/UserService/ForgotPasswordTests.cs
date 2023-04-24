using System.Net;
using AutoFixture;
using Identity.Application.Common.Exceptions;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Identity.UnitTests.Generators;
using Moq;

namespace Identity.UnitTests.Services.UserService;

public class ForgotPasswordTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_IdentityResultException_With_404_Status_Code_When_User_Not_Found()
    {
        //Arrange
        var parameters = Fixture.Create<ForgotPasswordParameters>();
        var forgotPasswordDto = new ForgotPasswordDto(parameters);

        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        //Act 
        var resultException =
            await Assert.ThrowsAsync<IdentityResultException>(() =>
                UserService.ForgotPasswordAsync(forgotPasswordDto, TestOrigin));

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, resultException.StatusCode);
    }

    [Fact]
    public async Task Should_Send_ResetPassword_Mail()
    {
        //Arrange
        var parameters = Fixture.Create<ForgotPasswordParameters>();
        var forgotPasswordDto = new ForgotPasswordDto(parameters);
        var user = Fixture.GenerateUser();
        UserRepositoryMock.Setup(_ => _.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act 
        var result = await UserService.ForgotPasswordAsync(forgotPasswordDto, TestOrigin);

        //Assert
        Assert.True(result.Success);
        IdentityEmailServiceMock.Verify(
            v => v.SendEmailResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}