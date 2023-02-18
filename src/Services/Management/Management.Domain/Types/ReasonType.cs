using Shared.Domain.Base;

namespace Management.Domain.Types;

public class ReasonType : Enumeration
{
    public static ReasonType OnDemand = new(1, nameof(OnDemand));
    public static ReasonType Rest = new(2, nameof(Rest));
    public static ReasonType Forced = new(3, nameof(Forced));
    public static ReasonType Free = new(4, nameof(Free));
    public static ReasonType Childcare = new(5, nameof(Childcare));

    protected ReasonType(int id, string name) : base(id, name)
    {
    }
}