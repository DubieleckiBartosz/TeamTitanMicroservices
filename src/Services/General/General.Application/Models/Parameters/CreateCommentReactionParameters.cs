using General.Application.Models.Enums;
using Newtonsoft.Json;

namespace General.Application.Models.Parameters;

public class CreateCommentReactionParameters
{
    public int CommentId { get; init; }
    public ReactionType Reaction { get; init; }

    public CreateCommentReactionParameters()
    {
    }

    [JsonConstructor]
    public CreateCommentReactionParameters(int commentId, ReactionType reaction)
    {
        CommentId = commentId;
        Reaction = reaction;
    }
}