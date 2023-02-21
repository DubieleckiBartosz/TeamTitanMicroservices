namespace Management.Domain.ValueObjects;

public class GiftRecipient
{
    public int? DepartmentId { get; }
    public int? EmployeeId { get; }

    private GiftRecipient(int? departmentId, int? employeeId)
    {
        DepartmentId = departmentId;
        EmployeeId = employeeId;
    }

    public GiftRecipient CreateGiftDepartment(int departmentId) => new GiftRecipient(departmentId, null);
    public GiftRecipient CreateGiftEmployee(int employeeId) => new GiftRecipient(null, employeeId);
}