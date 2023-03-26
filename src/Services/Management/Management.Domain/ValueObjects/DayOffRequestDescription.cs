using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class DayOffRequestDescription : ValueObject
{
    public string Value { get; }

    private DayOffRequestDescription(string value)
    {
        Value = value;
    }

    public static DayOffRequestDescription CreateReason(string value) => new DayOffRequestDescription(value);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Value;
    }
}