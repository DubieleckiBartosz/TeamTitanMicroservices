using Newtonsoft.Json;

namespace General.Application.Models.Parameters;

public class DeleteCommentReactionParameters
{
    public int CommentId { get; init; } 

    public DeleteCommentReactionParameters()
    {
    }

    [JsonConstructor]
    public DeleteCommentReactionParameters(int commentId)
    {
        CommentId = commentId;
    }
}