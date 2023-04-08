namespace Management.Application.Models.DataTransferObjects;

public class NewDayOffRequestMessageDto
{
    public string EmployeeFullName { get; init; }
    public string EmployeeCode { get; init; }

    public NewDayOffRequestMessageDto()
    {
    }

    public NewDayOffRequestMessageDto(string employeeFullName, string employeeCode)
    {
        EmployeeFullName = employeeFullName;
        EmployeeCode = employeeCode;
    }
}