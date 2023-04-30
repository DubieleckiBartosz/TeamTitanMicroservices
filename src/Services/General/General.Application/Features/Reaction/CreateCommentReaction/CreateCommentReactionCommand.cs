using General.Application.Models.Enums;
using General.Application.Models.Parameters.CommentParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Reaction.CreateCommentReaction;

public record CreateCommentReactionCommand(int CommentId, ReactionType Reaction) : ICommand<Unit>
{
    public static CreateCommentReactionCommand Create(CreateCommentReactionParameters parameters)
    {
        return new CreateCommentReactionCommand(parameters.CommentId, parameters.Reaction);
    }
}