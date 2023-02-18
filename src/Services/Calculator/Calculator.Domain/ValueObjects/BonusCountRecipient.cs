using Shared.Domain.Base;

namespace Calculator.Domain.ValueObjects;

public class BonusCountRecipient : ValueObject
{
    public string AddedBy { get; }
    public bool Settled { get; set; }//??
    public int Count { get; }

    public BonusCountRecipient(string addedBy, int count)
    {
        AddedBy = addedBy;
        Count = count;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.AddedBy;
        yield return this.Count;
    }
}