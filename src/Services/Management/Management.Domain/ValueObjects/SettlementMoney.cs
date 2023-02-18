using Shared.Domain.Base;

namespace Management.Domain.ValueObjects;

public class SettlementMoney : ValueObject
{
    public decimal Amount { get; }
    public decimal Bonus { get; } 
    private SettlementMoney(decimal amount, decimal bonus)
    {
        Amount = amount;
        Bonus = bonus; 
    }

    public static SettlementMoney Create(decimal amount, decimal bonus)
    {
        return new SettlementMoney(amount, bonus);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Amount;
        yield return this.Bonus;
    }
}