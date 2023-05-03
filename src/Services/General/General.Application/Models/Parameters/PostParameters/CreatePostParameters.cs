using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace General.Application.Models.Parameters.PostParameters;

public class CreatePostParameters
{
    public string Description { get; init; }
    public bool IsPublic { get; init; }
    public string Path { get; init; }
    public List<IFormFile>? Attachments { get; init; }
    public CreatePostParameters()
    {
    }

    [JsonConstructor]
    public CreatePostParameters(string description, bool isPublic, string path, List<IFormFile>? attachments)
    {
        Description = description;
        IsPublic = isPublic;
        Path = path;
        Attachments = attachments;
    }
}