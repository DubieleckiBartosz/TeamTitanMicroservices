using General.Application.Constants;
using General.Application.Contracts;
using General.Application.Validators.Logic;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Posts.Commands.DeletePost;

public class DeletePostHandler : ICommandHandler<DeletePostCommand, Unit>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;

    public DeletePostHandler(IPostRepository postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);
        if (post == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetByIdAsync"),
                ExceptionTitles.TitleNotFound("Post"));
        }

        post.ValidationAccessPostOperation(_currentUser);

        _postRepository.Remove(post);
        await _postRepository.SaveAsync();

        return Unit.Value;
    }
}