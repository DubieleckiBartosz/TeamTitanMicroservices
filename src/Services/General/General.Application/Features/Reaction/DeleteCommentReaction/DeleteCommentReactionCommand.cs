using General.Application.Models.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Reaction.DeleteCommentReaction;

public record DeleteCommentReactionCommand(int CommentId) : ICommand<Unit>
{
    public static DeleteCommentReactionCommand Create(DeleteCommentReactionParameters parameters)
    {
        return new DeleteCommentReactionCommand(parameters.CommentId);
    }
}