using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Application.Common;
using Identity.Application.Settings;

namespace Identity.Application.Utils;

public static class TokenUtils
{
    public static string CreateJwtToken(this User user, JwtSettings jwtSettings)
    {
        var roleClaims = new List<Claim>();
        roleClaims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToList());
        if (user.VerificationCode != null)
        {
            roleClaims.Add(new Claim(CommonConstants.ClaimCode, user.VerificationCode));
        } 

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, $"{user.UserName}"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }.Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public static string RandomTokenString()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }

    public static string CreateRefreshToken(this IPasswordHasher<User> passwordHasher, User user)
    {
        var refreshToken = passwordHasher.HashPassword(user, Guid.NewGuid().ToString())
            .Replace("+", string.Empty)
            .Replace("=", string.Empty)
            .Replace("/", string.Empty);

        return refreshToken;
    }
}