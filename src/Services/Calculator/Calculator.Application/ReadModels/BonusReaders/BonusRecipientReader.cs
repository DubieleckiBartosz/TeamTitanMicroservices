namespace Calculator.Application.ReadModels.BonusReaders;

public class BonusRecipientReader
{
    public int Count { get; private set; }
    public List<BonusReader> Bonuses { get; private set; } = new();

    private BonusRecipientReader()
    {
    }
    public static BonusRecipientReader Create()
    {
        return new BonusRecipientReader();
    }
    public void AddNewBonus(string creator)
    {
        var bonus = BonusReader.Create(creator);

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