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

    /// <summary>
    ///     For logic
    /// </summary>
    /// <param name="creator"></param>
    /// <param name="bonusCode"></param>
    /// <param name="recipient"></param>
    /// <param name="groupBonus"></param>
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

    /// <summary>
    ///     For load
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bonusCode"></param>
    /// <param name="groupBonus"></param>
    /// <param name="recipient"></param>
    /// <param name="creator"></param>
    /// <param name="settled"></param>
    /// <param name="canceled"></param>
    /// <param name="created"></param>
    private BonusReader(Guid id, string bonusCode, bool groupBonus, string recipient, string creator, bool settled,
        bool canceled, DateTime created) : this(creator, bonusCode, recipient, groupBonus)
    {
        Id = id;
        Settled = settled;
        Canceled = canceled;
        Created = created;
    }

    public static BonusReader Load(Guid id, string bonusCode, bool groupBonus, string recipient, string creator,
        bool settled,
        bool canceled, DateTime created)
    {
        return new BonusReader(id, bonusCode, groupBonus, recipient, creator, settled, canceled, created);
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