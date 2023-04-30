using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Posts.Commands.DeleteAttachment;

public class DeleteAttachmentHandler : ICommandHandler<DeleteAttachmentCommand, Unit>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteAttachmentHandler(IPostRepository postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
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

        post.RemoveAttachment(request.AttachmentTitle);
        _postRepository.Update(post);
        await _postRepository.SaveAsync();

        return Unit.Value;
    }
}