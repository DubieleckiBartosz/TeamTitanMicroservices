namespace Calculator.Domain.BonusProgram;

public class BonusCountRecipient 
{
    public int Count { get; private set; } 
    public List<Bonus> Bonuses { get; private set; } = new();

    private BonusCountRecipient()
    {
    }

    public static BonusCountRecipient Create()
    {
        return new BonusCountRecipient();
    }

    public void AddNewBonus(string creator)
    {
        var bonus = Bonus.Create(creator); 

        Bonuses.Add(bonus);
        Count++;
    }

    public void CancelBonus()
    {
        if (Count == 0)
        {
            return;
        }

        var result = Bonuses.Last();
        Bonuses.Remove(result);
        Count--;
    }
}