namespace Management.Application.Models.DataAccessObjects;

public class CommunicationDao
{ 
    public int Version { get; init; }
    public string City { get; init; } = default!;
    public string Street { get; init; } = default!;
    public string NumberStreet { get; init; } = default!;
    public string PostalCode { get; init; } = default!; 
    public string PhoneNumber { get; init; } = default!;
    public string Email { get; init; } = default!; 
}