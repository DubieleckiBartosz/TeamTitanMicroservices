using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;

namespace Identity.Infrastructure.AccessObjects;

internal class TokenDao
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime TokenExpirationDate { get; set; }
    public DateTime Created { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? Revoked { get; set; }

    public RefreshToken Map()
    {
        return RefreshToken.LoadToken(Id, Token, TokenExpirationDate, Created, ReplacedByToken, TokenActivity.Load(Revoked));
    }
}