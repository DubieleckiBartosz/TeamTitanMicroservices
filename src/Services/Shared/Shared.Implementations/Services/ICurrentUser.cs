namespace Shared.Implementations.Services;

public interface ICurrentUser
{
    bool IsInRole(string roleName);
    int UserId { get; } 
    string? VerificationCode { get; }
    string? OrganizationCode { get; }
    string UserName { get; }
    string Email { get; }
}