namespace Shared.Domain.Base;

public class Watcher
{
    public DateTime Created { get; }
    public DateTime Modified { get; private set; }

    private Watcher()
    {
        Created = DateTime.UtcNow;
        Modified = DateTime.UtcNow;
    }

    public Watcher(DateTime created, DateTime modified)
    {
        Created = created;
        Modified = modified;
    }

    public static Watcher Create() => new Watcher();
    public static Watcher Load(DateTime created, DateTime modified) => new Watcher(created, modified);

    public void Modify()
    {
        Modified = DateTime.UtcNow;
    }
}