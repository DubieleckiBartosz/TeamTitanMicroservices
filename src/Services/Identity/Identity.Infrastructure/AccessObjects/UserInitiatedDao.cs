using Identity.Domain.Entities;

namespace Identity.Infrastructure.AccessObjects;

public class UserInitiatedDao
{
    public int Id { get; set; }
    public string CompanyId { get; set; }
    public string DepartmentCode { get; set; }
    public string EmployeeCode { get; set; }
    public int Role { get; set; }

    public User Map()
    {
        return User.LoadUser(Id, CompanyId, DepartmentCode, EmployeeCode, Role);
    }
}