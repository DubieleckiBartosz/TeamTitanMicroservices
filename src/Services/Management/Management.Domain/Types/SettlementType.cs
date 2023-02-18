using Shared.Domain.Base;

namespace Management.Domain.Types;

public class SettlementType : Enumeration
{
    public static SettlementType Piecework = new(1, nameof(Piecework));
    public static SettlementType ForAnHour = new(2, nameof(ForAnHour));
    public static SettlementType FixedSalary = new(3, nameof(FixedSalary));

    protected SettlementType(int id, string name) : base(id, name)
    {
    }
}