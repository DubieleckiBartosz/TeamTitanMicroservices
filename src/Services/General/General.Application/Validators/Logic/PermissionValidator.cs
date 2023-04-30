using General.Application.Constants;
using General.Domain.Entities;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Validators.Logic;

public static class PermissionValidator
{
    public static void ValidationAccessPostOperation(this Post post, ICurrentUser currentUser)
    {
        var userMatch = post.CreatedBy == currentUser.UserId;

        var postOrganization = post.Organization != null;
        var userOrganizationMatch = postOrganization && post.Organization == currentUser.OrganizationCode;

        var userHasPermission = userMatch ||
                                currentUser.IsInRoles(new[] { UserAccess.Manager, UserAccess.Owner });

        if ((postOrganization && !currentUser.IsAdmin && (!userHasPermission || !userOrganizationMatch)) || (!userMatch && !currentUser.IsAdmin))
        {
            throw new NotFoundException(ExceptionDetails.DetailsNoPermissions,
                ExceptionTitles.TitleNoPermissions);
        }
    }
}