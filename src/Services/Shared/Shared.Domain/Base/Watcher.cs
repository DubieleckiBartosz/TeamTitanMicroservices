namespace Shared.Domain.Base;

public class Watcher
{
    public DateTime Created { get; init; }
    public DateTime? LastModified { get; private set; }

    private Watcher()
    {
        Created = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public static Watcher Create() => new Watcher();
    public void Update() => LastModified = DateTime.UtcNow;
}