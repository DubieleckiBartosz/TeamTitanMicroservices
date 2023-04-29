using Shared.Domain.Base;

namespace General.Domain.Types;

public class ReactionType : Enumeration
{
    public static ReactionType Like = new(1, nameof(Like));
    public static ReactionType Heart = new(2, nameof(Heart));
    public static ReactionType HaHa = new(3, nameof(HaHa));
    protected ReactionType(int id, string name) : base(id, name)
    {
    }
}