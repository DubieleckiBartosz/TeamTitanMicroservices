using General.Domain.Types;
using Shared.Domain.Base;

namespace General.Domain.ValueObjects;

public class Reaction : ValueObject
{
    public int Creator { get; private set; }
    public string CreatorName { get; private set; }
    public ReactionType Type { get; private set; }

    private Reaction()
    {
    }
    private Reaction(int creator, string creatorName, ReactionType type)
    {
        Creator = creator;
        CreatorName = creatorName;
        Type = type;
    }

    public static Reaction CreateReaction(int creator, string creatorName, int type)
        => new(creator, creatorName, Enumeration.GetById<ReactionType>(type));

    public override string ToString()
    {
        return Type.Name;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Creator;
        yield return CreatorName;
        yield return Type;
    }
}