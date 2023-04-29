using Newtonsoft.Json;

namespace General.Application.Models.Parameters;

public class DeleteCommentParameters
{
    public int PostId { get; init; } 
    public int CommentId { get; init; }

    public DeleteCommentParameters()
    {
    }

    [JsonConstructor]
    public DeleteCommentParameters(int postId, int commentId)
    {
        PostId = postId;
        CommentId = commentId;
    }
}