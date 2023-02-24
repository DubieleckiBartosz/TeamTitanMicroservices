using Shared.Domain.Base;

namespace Identity.Domain.ValueObjects;

public class TokenValue : ValueObject
{
    public string Token { get; }
    public DateTime? TokenExpirationDate { get; }
    public bool IsActive => TokenExpirationDate > DateTime.UtcNow;

    private TokenValue(string token, DateTime? tokenExpirationDate)
    {
        TokenExpirationDate = tokenExpirationDate;
        Token = token;
    }

    public static TokenValue Load(string token, DateTime? tokenExpirationDate = null)
    {
        return new TokenValue(token, tokenExpirationDate);
    }

    public static TokenValue CreateToken(string token)
    {
        CheckToken(token);
        return new TokenValue(token, DateTime.UtcNow.AddDays(5));
    }

    public static TokenValue CreateResetToken(string token)
    {
        CheckToken(token);
        return new TokenValue(token, DateTime.UtcNow.AddDays(1));
    }

    public static TokenValue CreateVerificationToken(string token)
    {
        CheckToken(token);
        return new TokenValue(token, DateTime.UtcNow.AddDays(2));
    }

    private static void CheckToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentNullException(nameof(token));
        }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Token;
        yield return TokenExpirationDate;
    }
}