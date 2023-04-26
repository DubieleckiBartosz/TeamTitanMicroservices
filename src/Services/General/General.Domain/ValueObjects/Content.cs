using Shared.Domain.Base;

namespace General.Domain.ValueObjects;

public class Content : ValueObject
{
    public string Description { get; private set; }

    private Content(string description)
    {
        Description = description;
    }

    public static Content Create(string description) => new (description);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Description;
    }
}