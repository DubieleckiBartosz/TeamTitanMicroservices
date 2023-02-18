using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class Contact : ValueObject
{
    public string PhoneNumber { get; }
    public string Email { get; }

    private Contact(string phoneNumber, string email)
    {
        PhoneNumber = phoneNumber;
        Email = email;
    }
    public static Contact Create(string phoneNumber, string email) => new(phoneNumber, email);
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.PhoneNumber;
        yield return this.Email;
    }
}