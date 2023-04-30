using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Reaction.DeletePostReaction;

public record DeletePostReactionCommand(int PostId) : ICommand<Unit>;