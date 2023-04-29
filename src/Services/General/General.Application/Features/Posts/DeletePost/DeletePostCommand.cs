using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Posts.DeletePost;

public record DeletePostCommand(int PostId) : ICommand<Unit>;