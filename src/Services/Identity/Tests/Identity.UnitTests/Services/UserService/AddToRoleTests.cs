using Identity.Application.Models.DataTransferObjects;

namespace Identity.UnitTests.Services.UserService;

public class AddToRoleTests : UserServiceBaseTests
{
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_Dto_Is_null()
    {
        //Act and assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => UserService.AddToRoleAsync(null as UserNewRoleDto));
    }

}