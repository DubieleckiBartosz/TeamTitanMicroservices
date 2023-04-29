using Newtonsoft.Json;

namespace General.Application.Models.Parameters;

public class UpdateCommentParameters
{
    public int CommentId { get; init; }
    public string NewContent { get; init; }

    public UpdateCommentParameters()
    {
    }

    [JsonConstructor]
    public UpdateCommentParameters(int commentId, string newContent)
    {
        CommentId = commentId;
        NewContent = newContent;
    }
}