namespace Shared.Implementations.Services;

public interface ICurrentUser
{
    bool IsInRole(string roleName);
    int UserId { get; }
    string? CompanyCode { get; }
    string? DepartmentCode { get; }
    string? EmployeeCode { get; }
    string UserName { get; }
    string Email { get; }
}