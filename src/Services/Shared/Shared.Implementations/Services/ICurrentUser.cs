namespace Shared.Implementations.Services;

public interface ICurrentUser
{
    bool IsInRole(string roleName);
    int UserId { get; }
    string? UserCode { get; }
    string UserName { get; }
    string Email { get; }
}