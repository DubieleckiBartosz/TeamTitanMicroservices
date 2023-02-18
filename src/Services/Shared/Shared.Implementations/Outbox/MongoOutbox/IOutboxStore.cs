namespace Shared.Implementations.Outbox.MongoOutbox;

public interface IOutboxStore
{
    Task AddAsync(OutboxMessage message);
    Task<IEnumerable<Guid>> GetUnprocessedMessageIdsAsync();
    Task SetMessageToProcessedAsync(Guid id);
    Task DeleteAsync(IEnumerable<Guid> ids);
    Task<OutboxMessage?> GetMessageAsync(Guid id);
}