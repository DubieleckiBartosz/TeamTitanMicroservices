using General.Application.Constants;
using General.Application.Contracts;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Comments.Commands.CreateComment;

public class CreateCommentHandler : ICommandHandler<CreateCommentCommand, int>
{
    private readonly IPostRepository _postBaseRepository;
    private readonly ICurrentUser _currentUser;

    public CreateCommentHandler(IPostRepository postRepository, ICurrentUser currentUser)
    {
        _postBaseRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _postBaseRepository.GetByIdAsync(request.PostId);
        if (post == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetByIdAsync"),
                ExceptionTitles.TitleNotFound("Post"));
        }

        var comment = post.AddComment(_currentUser.UserId, _currentUser.UserName, request.Comment);
        await _postBaseRepository.SaveAsync();

        return comment.Id;
    }
}