using Newtonsoft.Json;

namespace Management.Application.Parameters.DepartmentParameters;

public class CreateDepartmentParameters
{
    public string DepartmentName { get; init; }

    public CreateDepartmentParameters()
    {
    }

    [JsonConstructor]
    public CreateDepartmentParameters(string departmentName)
    {
        DepartmentName = departmentName;
    }
}
