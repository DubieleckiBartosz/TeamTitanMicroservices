using General.Application.Models.Enums;
using Newtonsoft.Json;

namespace General.Application.Models.Parameters.PostParameters;

public class CreatePostReactionParameters
{
    public int PostId { get; init; }
    public ReactionType Reaction { get; init; }

    public CreatePostReactionParameters()
    {
    }

    [JsonConstructor]
    public CreatePostReactionParameters(int postId, ReactionType reaction)
    {
        PostId = postId;
        Reaction = reaction;
    }
}