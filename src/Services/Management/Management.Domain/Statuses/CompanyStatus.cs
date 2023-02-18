using Shared.Domain.Base;

namespace Management.Domain.Statuses;

public class CompanyStatus : Enumeration
{
    public static CompanyStatus Active = new(1, nameof(Active));
    public static CompanyStatus Suspended = new(2, nameof(Suspended));
    public static CompanyStatus NotActive = new(3, nameof(NotActive));
    protected CompanyStatus(int id, string name) : base(id, name)
    {
    }
}