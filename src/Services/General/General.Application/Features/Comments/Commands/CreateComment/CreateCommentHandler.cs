using General.Application.Constants;
using General.Application.Contracts;
using General.Domain.Entities;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Comments.Commands.CreateComment;

public class CreateCommentHandler : ICommandHandler<CreateCommentCommand, int>
{
    private readonly IBaseRepository<Post> _postBaseRepository;
    private readonly ICurrentUser _currentUser;

    public CreateCommentHandler(IBaseRepository<Post> postBaseRepository, ICurrentUser currentUser)
    {
        _postBaseRepository = postBaseRepository ?? throw new ArgumentNullException(nameof(postBaseRepository));
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

        var comment = post.AddComment(_currentUser.UserId, request.Comment);
        await _postBaseRepository.SaveAsync();

        return comment.Id;
    }
}