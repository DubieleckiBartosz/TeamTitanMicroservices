using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace General.Application.Models.Parameters.PostParameters;

public class NewAttachmentParameters
{
    public int PostId { get; init; }
    public string Title { get; init; }
    //For tests
    public string Path { get; init; }
    public IFormFile Attachment { get; init; }

    public NewAttachmentParameters()
    {
    }

    [JsonConstructor]
    public NewAttachmentParameters(int postId, string title, string path, IFormFile attachment)
    {
        PostId = postId;
        Title = title;
        Path = path;
        Attachment = attachment;
    }
}