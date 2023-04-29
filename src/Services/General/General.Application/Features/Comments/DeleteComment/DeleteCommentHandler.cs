using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Comments.DeleteComment;

public class DeleteCommentHandler : ICommandHandler<DeleteCommentCommand, Unit>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteCommentHandler(IPostRepository postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostWithCommentsAsync(request.PostId);
        if (post == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetPostWithCommentsAsync"),
                ExceptionTitles.TitleNotFound("Post"));
        }

        var comment = post.GetCommentById(request.CommentId); 
        if (comment.Creator != _currentUser.UserId && _currentUser.IsAdmin!)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNoPermissions,
                ExceptionTitles.TitleNoPermissions); 
        }  

        post.RemoveComment(request.CommentId);

        await _postRepository.SaveAsync();

        return Unit.Value;
    }
}