using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Reaction.CreateCommentReaction;

public class CreateCommentReactionHandler : ICommandHandler<CreateCommentReactionCommand, Unit>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUser _currentUser;

    public CreateCommentReactionHandler(ICommentRepository commentRepository, ICurrentUser currentUser)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(CreateCommentReactionCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetCommentWithReactions(request.CommentId); 
        if (comment == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetCommentWithReactions"),
                ExceptionTitles.TitleNotFound("Comment"));
        }

        comment.AddNewReaction(_currentUser.UserId, (int) request.Reaction);

        _commentRepository.Update(comment);
        await _commentRepository.SaveAsync();

        return Unit.Value;
    }
}