namespace Calculator.Application.ReadModels.AccountReaders;

public class BonusReader
{
    public Guid Id { get; }
    public string BonusCode { get; } 
    public string Creator { get; }
    public bool Settled { get; private set; }
    public bool Canceled { get; private set; }
    public DateTime Created { get; }

    /// <summary>
    ///     For logic
    /// </summary>
    /// <param name="creator"></param>
    /// <param name="bonusCode"></param>
    private BonusReader(string creator, string bonusCode)
    { 
        BonusCode = bonusCode;
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
    private BonusReader(Guid id, string bonusCode, string creator, bool settled,
        bool canceled, DateTime created) : this(creator, bonusCode)
    {
        Id = id;
        Settled = settled;
        Canceled = canceled;
        Created = created;
    }

    public static BonusReader Load(Guid id, string bonusCode, string creator,
        bool settled,
        bool canceled, DateTime created)
    {
        return new BonusReader(id, bonusCode, creator, settled, canceled, created);
    }

    public static BonusReader Create(string creator, string bonusCode)
    {
        return new BonusReader(creator, bonusCode);
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