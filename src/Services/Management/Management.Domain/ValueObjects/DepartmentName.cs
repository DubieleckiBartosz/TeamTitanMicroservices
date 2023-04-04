using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class DepartmentName : ValueObject
{
    public string Value { get; }

    private DepartmentName(string value)
    {
        Value = value;
    }

    public static DepartmentName Create(string value)
    {
        return new(value);
    }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Value;
    }

    public virtual bool Equals(DepartmentName? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        DepartmentName other = (DepartmentName)obj;
        return Value == other.Value;
    } 
}