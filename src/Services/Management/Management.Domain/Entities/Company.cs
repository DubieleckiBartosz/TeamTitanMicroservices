namespace Management.Domain.Entities;

public class Company
{
    public string CompanyName { get; }
    public string CompanyExternalId { get; }
    public string OwnerId { get; }
    public List<Department> Departments { get; private set; }

    private Company(string companyName, string companyExternalId, string ownerId)
    {
        CompanyName = companyName;
        CompanyExternalId = companyExternalId;
        OwnerId = ownerId;
    }
}