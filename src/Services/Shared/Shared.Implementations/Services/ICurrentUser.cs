namespace Shared.Implementations.Services;

public interface ICurrentUser
{
    bool IsAdmin { get; }
    bool IsInRole(string roleName);
    bool IsInRoles(string[] roles);
    int UserId { get; } 
    string? VerificationCode { get; }
    string? OrganizationCode { get; }
    string UserName { get; }
    string Email { get; }
}