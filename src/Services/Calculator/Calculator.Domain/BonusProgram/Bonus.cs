namespace Calculator.Domain.BonusProgram;

public class Bonus
{
    public string Creator { get; }
    public bool Settled { get; private set; }
    public DateTime SettlementTime { get; }
    public DateTime Created { get; }

    private Bonus(DateTime settlementTime, string creator)
    {
        Settled = false;
        SettlementTime = settlementTime;
        Created = DateTime.UtcNow;
        Creator = creator;
    }

    public static Bonus Create(DateTime settlementTime, string creator) => new Bonus(settlementTime, creator);

    public void AsSettled()
    {
        Settled = true;
    }
}