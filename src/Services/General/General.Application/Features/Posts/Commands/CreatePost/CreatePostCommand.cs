using Shared.Implementations.Abstractions;

namespace General.Application.Features.Posts.Commands.CreatePost;

public record CreatePostCommand(string Description, bool IsPublic) : ICommand<int>
{
}