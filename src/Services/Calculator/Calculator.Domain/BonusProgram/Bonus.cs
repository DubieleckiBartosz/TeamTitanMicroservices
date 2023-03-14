namespace Calculator.Domain.BonusProgram;

public class Bonus
{
    public string BonusCode { get; }
    public bool GroupBonus { get; }

    //Department or individual employee
    public string Recipient { get; }
    public string Creator { get; }
    public bool Settled { get; private set; }
    public bool Canceled { get; private set; }
    public DateTime Created { get; }

    private Bonus(string creator, string bonusCode, string recipient, bool groupBonus)
    {
        GroupBonus = groupBonus;
        Recipient = recipient;
        BonusCode = bonusCode;
        Settled = false;
        Canceled = false;
        Created = DateTime.UtcNow;
        Creator = creator;
    }

    public static Bonus Create(string creator, string bonusCode, string recipient, bool groupBonus)
    {
        return new Bonus(creator, bonusCode, recipient, groupBonus);
    }

    public Bonus AsSettled()
    {
        Settled = true;
        return this;
    }

    public Bonus AsCanceled()
    {
        Canceled = true;
        return this;
    }
}