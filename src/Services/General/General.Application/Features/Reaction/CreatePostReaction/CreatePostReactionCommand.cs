using General.Application.Models.Enums;
using General.Application.Models.Parameters.PostParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Reaction.CreatePostReaction;

public record CreatePostReactionCommand(int PostId, ReactionType Reaction) : ICommand<Unit>
{
    public static CreatePostReactionCommand Create(CreatePostReactionParameters parameters)
    {
        return new CreatePostReactionCommand(parameters.PostId, parameters.Reaction);
    }
}