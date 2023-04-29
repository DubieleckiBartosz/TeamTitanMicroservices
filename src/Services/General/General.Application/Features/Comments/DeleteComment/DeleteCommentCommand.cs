using General.Application.Models.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Comments.DeleteComment;

public record DeleteCommentCommand(int PostId, int CommentId) : ICommand<Unit>
{
    public static DeleteCommentCommand Create(DeleteCommentParameters parameters)
    {
        return new DeleteCommentCommand(parameters.PostId, parameters.CommentId);
    }
}