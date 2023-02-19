namespace Calculator.Domain.BonusProgram;

public class BonusCountRecipient 
{
    public int Count { get; private set; }
    public int All => Bonuses.Count;
    public List<Bonus> Bonuses { get; private set; } = new(); 

    public void AddNewBonus(DateTime settlementTime, string creator)
    {
        var bonus = Bonus.Create(settlementTime, creator); 

        Bonuses.Add(bonus);
        Count++;
    }

    public void RemoveBonus()
    { 
    }
}