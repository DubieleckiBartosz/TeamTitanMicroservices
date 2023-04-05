namespace Management.Application.Models.DataAccessObjects;

public class CompanyDao
{
    public int OwnerId { get; init; }
    public string City { get; init; } = default!;
    public string Street { get; init; } = default!;
    public string NumberStreet { get; init; } = default!;
    public string PostalCode { get; init; } = default!;
    public string CompanyCode { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string OwnerCode { get; init; } = default!;
    public bool IsConfirmed { get; init; }
    public string CompanyName { get; init; } = default!;
    public int From { get; init; }
    public int To { get; init; }
    public int CompanyStatus { get; init; }
    public List<DepartmentDao> Departments { get; init; } = new();
}