using Shared.Domain.Base;

namespace Management.Domain.Types;

public class ContractType : Enumeration
{ 
    public static ContractType ContractWork = new(1, nameof(ContractWork));
    public static ContractType ContractEmploymentLimitedPeriod = new(2, nameof(ContractEmploymentLimitedPeriod));
    public static ContractType ContractEmploymentIndefinitePeriod = new(3, nameof(ContractEmploymentIndefinitePeriod)); 

    protected ContractType(int id, string name) : base(id, name)
    {
    }
}