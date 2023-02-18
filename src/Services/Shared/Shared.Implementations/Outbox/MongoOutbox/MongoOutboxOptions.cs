namespace Shared.Implementations.Outbox.MongoOutbox;

public class MongoOutboxOptions
{
    public bool DeleteAfter { get; set; }
    public string CollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}