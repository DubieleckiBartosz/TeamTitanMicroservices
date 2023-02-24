using Shared.Domain.Base;

namespace Identity.Domain.ValueObjects;

public class TokenActivity : ValueObject
{
    public string? Revoked { get; }

    private TokenActivity(string? revoked)
    {
        Revoked = revoked;
    }
    public static TokenActivity Load(string? revoked) => new TokenActivity(revoked);
    public static TokenActivity IsNotRevoked() => new TokenActivity(null);
    public static TokenActivity IsRevoked() => new TokenActivity(DateTime.UtcNow.ToLongTimeString());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Revoked;
    }
}