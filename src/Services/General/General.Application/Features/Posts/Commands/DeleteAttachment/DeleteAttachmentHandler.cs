using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.FileOperations;
using Shared.Implementations.Services;

namespace General.Application.Features.Posts.Commands.DeleteAttachment;

public class DeleteAttachmentHandler : ICommandHandler<DeleteAttachmentCommand, Unit>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IFileService _fileService;

    public DeleteAttachmentHandler(IPostRepository postRepository, ICurrentUser currentUser, IFileService fileService)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
    }
    public async Task<Unit> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostWithAttachmentsAsync(request.PostId);

        if (post == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetPostWithAttachments"),
                ExceptionTitles.TitleNotFound("Post"));
        }

        if (post.CreatedBy != _currentUser.UserId && !_currentUser.IsAdmin)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNoPermissions,
                ExceptionTitles.TitleNoPermissions);
        }

        var result = post.RemoveAttachment(request.AttachmentTitle);
        var fullPath = Path.Combine(result.Path, result.Title);

        _postRepository.Update(post);

        await _postRepository.SaveAsync();


        _fileService.RemoveFile(fullPath);

        return Unit.Value;
    }
}