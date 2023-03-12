namespace Calculator.Application.ReadModels.BonusReaders;

public class BonusReader
{
    public Guid Id { get; }
    public string BonusCode { get; }
    public bool GroupBonus { get; }
    public string Recipient { get; }
    public string Creator { get; }
    public bool Settled { get; private set; }
    public bool Canceled { get; private set; }
    public DateTime Created { get; }

    private BonusReader(string creator, string bonusCode, string recipient, bool groupBonus)
    {
        GroupBonus = groupBonus;
        Recipient = recipient;
        BonusCode = bonusCode;
        Settled = false;
        Canceled = false;
        Created = DateTime.UtcNow;
        Creator = creator;
    }

    public static BonusReader Create(string creator, string bonusCode, string recipient, bool groupBonus)
    {
        return new BonusReader(creator, bonusCode, recipient, groupBonus);
    }

    public BonusReader AsSettled()
    {
        Settled = true;
        return this;
    }

    public BonusReader AsCanceled()
    {
        Canceled = true;
        return this;
    }
}