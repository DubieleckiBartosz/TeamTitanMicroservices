namespace Shared.Domain.Base;

public class Watcher
{
    public DateTime Created { get; init; }
    public DateTime? LastModified { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Watcher()
    {
    }

    public static Watcher Create() => new Watcher()
    {

        Created = DateTime.UtcNow,
        LastModified = DateTime.UtcNow,
        DeletedAt = null
    };

    public void Update() => LastModified = DateTime.UtcNow;
    public void Delete() => DeletedAt = DateTime.UtcNow;
}