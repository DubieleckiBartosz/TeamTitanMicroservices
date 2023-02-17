namespace Management.Domain.Types;

public record ContractType(string Value)
{
    public const string None = nameof(None);
    public const string ContractWork = nameof(ContractWork);
    public const string ContractEmploymentLimitedPeriod = nameof(ContractEmploymentLimitedPeriod);
    public const string ContractEmploymentIndefinitePeriod = nameof(ContractEmploymentIndefinitePeriod);

    public static implicit operator string(ContractType contractType)
        => contractType.Value;

    public static implicit operator ContractType(string value)
        => new(value);
}