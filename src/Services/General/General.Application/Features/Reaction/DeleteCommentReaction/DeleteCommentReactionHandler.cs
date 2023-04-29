using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Reaction.DeleteCommentReaction;

public class DeleteCommentReactionHandler : ICommandHandler<DeleteCommentReactionCommand, Unit>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteCommentReactionHandler(ICommentRepository commentRepository, ICurrentUser currentUser)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(DeleteCommentReactionCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetCommentWithReactions(request.CommentId);
        if (comment == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetCommentWithReactions"),
                ExceptionTitles.TitleNotFound("Comment"));
        }

        comment.RemoveReaction(_currentUser.UserId);

        _commentRepository.Update(comment);
        await _commentRepository.SaveAsync();

        return Unit.Value;
    }
}