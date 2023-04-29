using General.Application.Models.Parameters;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Comments.Commands.CreateComment;

public record CreateCommentCommand(int PostId, string Comment) : ICommand<int>
{
    public static CreateCommentCommand Create(CreateCommentParameters parameters)
    {
        return new CreateCommentCommand(parameters.PostId, parameters.Comment);
    }
}