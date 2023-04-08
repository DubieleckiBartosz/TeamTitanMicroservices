using Newtonsoft.Json;

namespace Management.Application.Parameters.EmployeeParameters;

public class UpdateEmployeeLeaderParameters
{
    public int EmployeeId { get; init; }
    public string NewLeader { get; init; }

    public UpdateEmployeeLeaderParameters()
    {
    }

    [JsonConstructor]
    public UpdateEmployeeLeaderParameters(int employeeId, string newLeader)
    {
        EmployeeId = employeeId;
        NewLeader = newLeader;
    }
}