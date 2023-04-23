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

        return User.CreateUser(verificationToken, userName, email, phoneNumber);
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
}