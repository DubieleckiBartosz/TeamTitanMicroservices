namespace Identity.Application.Models.DataTransferObjects;

public class UserCurrentIFullInfoDto
{
    public string? DepartmentCode { get; private set; }
    public string? CompanyId { get; private set; }
    public string? EmployeeCode { get; private set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Roles { get; set; }

    public UserCurrentIFullInfoDto(string? departmentCode, string? companyId, string? employeeCode, string userName,
        string email, string phoneNumber,
        List<string> roles)
    {
        DepartmentCode = departmentCode;
        CompanyId = companyId;
        EmployeeCode = employeeCode;
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
        Roles = roles;
    }
}