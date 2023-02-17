namespace Management.Domain.ValueObjects;

public record SettlementMoney
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
}