using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Comments.UpdateComment;

public class UpdateCommentHandler : ICommandHandler<UpdateCommentCommand, Unit>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUser _currentUser;

    public UpdateCommentHandler(ICommentRepository commentRepository, ICurrentUser currentUser)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId);
        if (comment == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetByIdAsync"),
                ExceptionTitles.TitleNotFound("Comment"));
        }

        if (comment.Creator != _currentUser.UserId)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNoPermissions,
                ExceptionTitles.TitleNoPermissions);
        }

        comment.UpdateContent(request.NewContent);

        _commentRepository.Update(comment);
        await _commentRepository.SaveAsync();

        return Unit.Value;
    }
}