using Newtonsoft.Json;

namespace General.Application.Models.Parameters;

public class CreatePostParameters
{ 
    public string Description { get; init; }
    public bool IsPublic { get; init; }

    public CreatePostParameters()
    {
    }

    [JsonConstructor]
    public CreatePostParameters(string description, bool isPublic)
    {
        Description = description;
        IsPublic = isPublic;
    }
}