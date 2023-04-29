using General.Application.Models.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Comments.UpdateComment;

public record UpdateCommentCommand(int CommentId, string NewContent) : ICommand<Unit>
{
    public static UpdateCommentCommand Create(UpdateCommentParameters parameters)
    {
        return new UpdateCommentCommand(parameters.CommentId, parameters.NewContent);
    }
}