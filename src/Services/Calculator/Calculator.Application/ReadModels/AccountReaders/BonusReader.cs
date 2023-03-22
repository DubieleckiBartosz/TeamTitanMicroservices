namespace Calculator.Application.ReadModels.AccountReaders;

public class BonusReader
{
    public Guid Id { get; }
    public string BonusCode { get; }
    public decimal Amount { get; }
    public string Creator { get; }
    public bool Settled { get; private set; }
    public bool Canceled { get; private set; }
    public DateTime Created { get; }

    /// <summary>
    ///     For logic
    /// </summary>
    /// <param name="creator"></param>
    /// <param name="bonusCode"></param>
    /// <param name="amount"></param>
    private BonusReader(string creator, string bonusCode, decimal amount)
    { 
        BonusCode = bonusCode;
        Amount = amount;
        Settled = false;
        Canceled = false;
        Created = DateTime.UtcNow;
        Creator = creator;
    }

    /// <summary>
    ///     For load
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bonusCode"></param>
    /// <param name="creator"></param>
    /// <param name="settled"></param>
    /// <param name="canceled"></param>
    /// <param name="created"></param>
    /// <param name="amount"></param>
    private BonusReader(Guid id, string bonusCode, string creator, bool settled,
        bool canceled, DateTime created, decimal amount) : this(creator, bonusCode, amount)
    {
        Id = id;
        Settled = settled;
        Canceled = canceled;
        Created = created;
    }

    public static BonusReader Load(Guid id, string bonusCode, string creator,
        bool settled,
        bool canceled, DateTime created, decimal amount)
    {
        return new BonusReader(id, bonusCode, creator, settled, canceled, created, amount);
    }

    public static BonusReader Create(string creator, string bonusCode, decimal amount)
    {
        return new BonusReader(creator, bonusCode, amount);
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