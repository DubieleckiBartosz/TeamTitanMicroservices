using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class CompanyName : ValueObject
{
    public string Value { get; }

    private CompanyName(string value)
    {
        Value = value;
    }

    public static CompanyName Create(string value) => new CompanyName(value);
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Value;
    } 
    public override string ToString()
    {
        return Value;
    }
}