using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Reaction.DeleteCommentReaction;

public record DeleteCommentReactionCommand(int CommentId) : ICommand<Unit>;