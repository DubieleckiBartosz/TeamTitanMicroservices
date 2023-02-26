using Identity.Domain.Entities;

namespace Identity.Infrastructure.AccessObjects;

public class UserInitiatedDao
{
    public int Id { get; set; } 
    public string VerificationCode { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }

    public User Map()
    {
        return User.LoadUser(Id, VerificationCode, Role, Email);
    }
}