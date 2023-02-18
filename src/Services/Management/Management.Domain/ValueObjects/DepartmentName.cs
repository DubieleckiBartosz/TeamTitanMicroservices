using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class DepartmentName : ValueObject
{
    public string Value { get; }

    private DepartmentName(string value)
    {
        Value = value;
    }

    public static DepartmentName Create(string value) => new DepartmentName(value);
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Value;
    }
}