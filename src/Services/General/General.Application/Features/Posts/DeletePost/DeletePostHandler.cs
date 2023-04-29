using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Posts.DeletePost;

public class DeletePostHandler : ICommandHandler<DeletePostCommand, Unit>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;

    public DeletePostHandler(IPostRepository postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);
        if (post == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetByIdAsync"),
                ExceptionTitles.TitleNotFound("Post"));
        }

        var userMatch = post.CreatedBy == _currentUser.UserId;
      
        var postOrganization = post.Organization != null;
        var userOrganizationMatch = postOrganization && post.Organization == _currentUser.OrganizationCode;

        var userHasPermission = userMatch ||
                                _currentUser.IsInRoles(new[] { UserAccess.Manager, UserAccess.Owner });

        if ((postOrganization && !_currentUser.IsAdmin && (!userHasPermission || !userOrganizationMatch)) || (!userMatch && !_currentUser.IsAdmin))
        {
            throw new NotFoundException(ExceptionDetails.DetailsNoPermissions,
                ExceptionTitles.TitleNoPermissions);
        } 
        _postRepository.Remove(post);
        await _postRepository.SaveAsync();

        return Unit.Value;
    }
}