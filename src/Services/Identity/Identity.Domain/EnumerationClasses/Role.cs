using Shared.Domain.Base;

namespace Identity.Domain.EnumerationClasses;

public class Role : Enumeration
{
    public static Role Admin = new(1, nameof(Admin));
    public static Role Owner = new(2, nameof(Owner));
    public static Role Manager = new(3, nameof(Manager));
    public static Role Employee = new(4, nameof(Employee));
    public static Role User = new(5, nameof(User));

    protected Role(int id, string name) : base(id, name)
    {
    }
}