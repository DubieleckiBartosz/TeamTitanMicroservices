using Shared.Implementations.Abstractions;

namespace General.Application.Features.Posts.CreatePost;

public record CreatePostCommand(string Description, bool IsPublic) : ICommand<int>
{
}