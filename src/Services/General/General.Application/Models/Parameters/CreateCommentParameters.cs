using Newtonsoft.Json;

namespace General.Application.Models.Parameters;

public class CreateCommentParameters
{
    public int PostId { get; init; }
    public string Comment { get; init; }

    public CreateCommentParameters()
    {
    }

    [JsonConstructor]
    public CreateCommentParameters(int postId, string comment)
    {
        PostId = postId;
        Comment = comment;
    }
}