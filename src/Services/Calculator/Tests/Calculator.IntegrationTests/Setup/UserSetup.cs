using System.Security.Claims;
using Calculator.IntegrationTests.Settings;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Calculator.IntegrationTests.Setup;
public enum FakeRoles
{
    Admin = 1,
    Owner = 2,
    Manager = 3,
    Employee = 4,
    User = 5
}

public class UserSetup
{
    public static ClaimsPrincipal UserPrincipals(bool isInOrganization = true)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        var userName = GlobalSettings.UserName;
        var userVerificationCode = GlobalSettings.UserVerificationCode;
        var organizationCode = GlobalSettings.OrganizationCode;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, FakeRoles.User.ToString()),
            new Claim(ClaimTypes.Role, FakeRoles.Manager.ToString()),
            new Claim(ClaimTypes.Role, FakeRoles.Admin.ToString()),
            new Claim(ClaimTypes.Role, FakeRoles.Owner.ToString()),
            new Claim(ClaimTypes.Role, FakeRoles.Employee.ToString()), 
            new Claim(ClaimTypes.Name, $"{userName}"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "SuperUser@test.com"),
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

        if (isInOrganization)
        { 
            claims.Add(new Claim("user_organization_code", organizationCode));
            claims.Add(new Claim("user_verification_code", userVerificationCode));
        }

        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims));

        return claimsPrincipal;
    }
}