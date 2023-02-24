using Identity.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Identity.Domain.Entities;

public class RefreshToken : Entity
{
    public TokenValue TokenValue { get; }
    public DateTime Created { get; }
    public string? ReplacedByToken { get; private set; }
    public TokenActivity TokenActivity { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= TokenValue.TokenExpirationDate;
    public bool IsActive => TokenActivity.Revoked == null && !IsExpired;

    private RefreshToken(TokenValue token)
    {
        TokenValue = token;
        Created = DateTime.UtcNow;
        TokenActivity = TokenActivity.IsNotRevoked();
    }

    private RefreshToken(int id, string token, DateTime tokenExpirationDate, DateTime created, string? replacedByToken,
        TokenActivity tokenActivity) :
        this(TokenValue.Load(token, tokenExpirationDate))
    {
        Id = id;
        Created = created;
        ReplacedByToken = replacedByToken;
        TokenActivity = tokenActivity;
    }

    public static RefreshToken LoadToken(int id, string token, DateTime tokenExpirationDate, DateTime created,
        string? replacedByToken,
        TokenActivity tokenActivity)
    {
        return new RefreshToken(id, token, tokenExpirationDate, created, replacedByToken, tokenActivity);
    }

    public static RefreshToken CreateToken(string token)
    {
        var tokenValue = TokenValue.CreateToken(token);
        return new RefreshToken(tokenValue);
    }

    public string GetTokenValue() => TokenValue.Token;

    public DateTime GetTokenExpirationDate()
    {
        return TokenValue.TokenExpirationDate ??
               throw new ArgumentNullException(nameof(TokenValue.TokenExpirationDate));
    }

    public void ReplaceToken(string newToken)
    {
        ReplacedByToken = newToken;
    }

    public void RevokeToken()
    {
        TokenActivity = TokenActivity.IsRevoked();
    }
}