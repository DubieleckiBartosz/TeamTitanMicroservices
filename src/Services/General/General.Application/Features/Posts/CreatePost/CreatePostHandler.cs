using General.Application.Contracts;
using General.Domain.Entities;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Services;

namespace General.Application.Features.Posts.CreatePost;

public class CreatePostHandler : ICommandHandler<CreatePostCommand, int>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;

    public CreatePostHandler(IPostRepository postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var organization = request.IsPublic ? null : _currentUser.OrganizationCode;
        var newPost = Post.Create(_currentUser.UserId, request.Description, request.IsPublic, organization);

        await _postRepository.AddAsync(newPost);

        await _postRepository.SaveAsync();

        return newPost.Id;
    }
}