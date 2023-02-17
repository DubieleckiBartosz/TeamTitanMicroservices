namespace Management.Domain.Entities;

public class Employee
{
    public string Name { get; }
    public string Surname { get; }
    public DateTime Birthday { get; }
    public string PersonalIdentificationNumber { get; }
    public List<EmployeeContract> Contracts { get; set; }
    public List<DayOffRequest> DayOffRequests { get; set; }
}