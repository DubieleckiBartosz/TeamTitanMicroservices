using General.Application.Models.Parameters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Posts.Commands.NewAttachment;

public record NewAttachmentCommand(int PostId, IFormFile Attachment, string Path, string Title) : ICommand<Unit>
{
    public static NewAttachmentCommand Create(NewAttachmentParameters parameters)
    {
        return new NewAttachmentCommand(parameters.PostId, parameters.Attachment, parameters.Path, parameters.Title);
    }
}