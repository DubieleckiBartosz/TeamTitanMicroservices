using General.Application.Constants;
using General.Application.Contracts;
using General.Domain.ValueObjects;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.FileOperations;

namespace General.Application.Features.Posts.NewAttachment;

public class NewAttachmentHandler : ICommandHandler<NewAttachmentCommand, Unit>
{
    private readonly IPostRepository _postRepository;
    private readonly IFileService _fileService;

    public NewAttachmentHandler(IPostRepository postRepository, IFileService fileService)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
    }
    public async Task<Unit> Handle(NewAttachmentCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostWithAttachments(request.PostId);

        if (post == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetPostWithAttachments"),
                ExceptionTitles.TitleNotFound("Post"));
        }

        var attachment = Attachment.Create(request.Title, request.Path);
        post.AddAttachment(attachment);

        _postRepository.Update(post);

        var path = string.Empty;
        try
        {
            path = await _fileService.SaveFileAsync(request.Attachment, request.Path);
            await _postRepository.SaveAsync();
        }
        catch
        {
            if (!string.IsNullOrEmpty(path))
            {
                _fileService.RemoveFile(path);
            }

            throw;
        }
        
        return Unit.Value;
    }
}