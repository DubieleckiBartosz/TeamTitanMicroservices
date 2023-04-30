using General.Application.Models.Parameters.PostParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Posts.Commands.DeleteAttachment;

public record DeleteAttachmentCommand(int PostId, string AttachmentTitle) : ICommand<Unit>
{
    public static DeleteAttachmentCommand Create(DeleteAttachmentParameters parameters)
    {
        return new DeleteAttachmentCommand(parameters.PostId, parameters.AttachmentTitle);
    }
}