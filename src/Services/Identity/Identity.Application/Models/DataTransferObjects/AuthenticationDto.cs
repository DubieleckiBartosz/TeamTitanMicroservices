using System.Text.Json.Serialization;

namespace Identity.Application.Models.DataTransferObjects;

public class AuthenticationDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public string Token { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }

    public AuthenticationDto()
    {
    }

    public AuthenticationDto(string userName, string email, List<string> roles, string token, string refreshToken,
        DateTime refreshTokenExpiration)
    {
        UserName = userName;
        Email = email;
        Roles = roles;
        Token = token;
        RefreshToken = refreshToken;
        RefreshTokenExpiration = refreshTokenExpiration;
    }
}