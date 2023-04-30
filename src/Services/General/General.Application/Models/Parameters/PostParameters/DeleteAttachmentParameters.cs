using Newtonsoft.Json;

namespace General.Application.Models.Parameters.PostParameters;

public class DeleteAttachmentParameters
{
    public int PostId { get; init; }
    public string AttachmentTitle { get; init; }

    public DeleteAttachmentParameters()
    {
    }

    [JsonConstructor]
    public DeleteAttachmentParameters(int postId, string attachmentTitle)
    {
        PostId = postId;
        AttachmentTitle = attachmentTitle;
    }
}