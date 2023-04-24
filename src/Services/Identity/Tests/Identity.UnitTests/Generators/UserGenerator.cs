using AutoFixture;
using Identity.Domain.Entities;

namespace Identity.UnitTests.Generators;

public static class UserGenerator
{
    public static User GenerateUser(this Fixture fixture)
    {
        var verificationToken = fixture.Create<string>();
        var userName = fixture.Create<string>();
        var email = fixture.Create<string>();
        var phoneNumber = fixture.Create<string>();

        var user = User.CreateUser(verificationToken, userName, email, phoneNumber);
        var somePassword = fixture.Create<string>();
        user.SetPasswordHash(somePassword);

        return user;
    }

    public static User GenerateUserCompany(this Fixture fixture, string? organizationCode = null, string? verificationCode = null)
    {
        var user = fixture.GenerateUser();
        organizationCode ??= fixture.Create<string>();
        verificationCode ??= fixture.Create<string>();

        user.AssignCodes(verificationCode, organizationCode);

        return user;
    }

    public static User GenerateApprovedUser(this Fixture fixture)
    {
        var user = fixture.GenerateUser();
        user.ConfirmAccount();

        return user;
    }

    public static User GenerateApprovedUserWithActiveRefreshToken(this Fixture fixture)
    {
        var user = fixture.GenerateUser();
        var newToken = fixture.Create<string>();

        user.ConfirmAccount();
        user.AddNewRefreshToken(newToken);

        return user;
    }
    
    public static User GenerateApprovedUserWithResetToken(this Fixture fixture)
    {
        var user = fixture.GenerateApprovedUserWithActiveRefreshToken();
        var resetToken = fixture.Create<string>();

        user.SetResetToken(resetToken);

        return user;
    }
}