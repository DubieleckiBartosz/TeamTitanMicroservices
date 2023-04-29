using Shared.Implementations.Abstractions;

namespace General.Application.Features.Posts.CreatePost;

public class CreatePostHandler : ICommandHandler<CreatePostCommand, int>
{
    public Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}