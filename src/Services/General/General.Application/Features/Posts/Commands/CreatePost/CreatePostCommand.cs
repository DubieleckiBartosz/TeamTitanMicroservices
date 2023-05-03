using General.Application.Models.Parameters.PostParameters;
using Microsoft.AspNetCore.Http;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Posts.Commands.CreatePost;

public record CreatePostCommand(string Description, bool IsPublic, List<IFormFile>? Attachments, string Path) : ICommand<int>
{
    public static CreatePostCommand Create(CreatePostParameters parameters)
    {
        return new CreatePostCommand(parameters.Description, parameters.IsPublic, parameters.Attachments, parameters.Path);
    }
}