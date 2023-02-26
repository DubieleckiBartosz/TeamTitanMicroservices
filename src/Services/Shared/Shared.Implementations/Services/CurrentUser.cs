using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;
using Shared.Implementations.Core.Exceptions;

namespace Shared.Implementations.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    private ClaimsPrincipal? Claims => _httpContextAccessor?.HttpContext?.User;
    public bool IsInRole(string roleName)
    {
        var resultRoles = Claims != null ? null : Claims?.Claims.Where(_ => _.Type == ClaimTypes.Role).ToList();
        var response = resultRoles?.Any(_ => _.Value == roleName);
        return response ?? false;
    }

    public int UserId
    {
        get
        {
            var result = Claims?.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
            if (result == null)
            {
                return default;
            }

            return int.TryParse(result, out var identifier) ? identifier : default;
        }
    } 
    public string? VerificationCode
    {
        get
        {
            return Claims?.Claims.FirstOrDefault(_ => _.Type == Constants.ClaimCodeType)?.Value;
        }
    }

    public string UserName
    {
        get
        {
            var result = Claims?.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name)?.Value;
            if (result == null)
            {
                throw new TeamTitanApplicationException("User name cannot be null", "User name is null",
                    HttpStatusCode.Unauthorized);
            }

            return result;
        }
    }
    public string Email
    {
        get
        {
            var result = Claims?.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email)?.Value;
            if (result == null)
            {
                throw new TeamTitanApplicationException("User mail cannot be null", "User mail is null",
                    HttpStatusCode.Unauthorized);
            }

            return result;
        }
    }
}