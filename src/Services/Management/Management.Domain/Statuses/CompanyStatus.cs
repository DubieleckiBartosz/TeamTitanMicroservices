using Shared.Domain.Base;

namespace Management.Domain.Statuses;

public class CompanyStatus : Enumeration
{
    public static CompanyStatus Init = new(1, nameof(Init));
    public static CompanyStatus Active = new(2, nameof(Active));
    public static CompanyStatus Suspended = new(3, nameof(Suspended));
    public static CompanyStatus NotActive = new(4, nameof(NotActive));
    protected CompanyStatus(int id, string name) : base(id, name)
    {
    }
}