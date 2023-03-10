using Shared.Domain.Base;

namespace Shared.Implementations.EventStore.Repositories;

public interface IRepository<TAggregate> where TAggregate : Aggregate
{
    Task<TAggregate?> GetAggregateFromSnapshotAsync(Guid id);
    Task<TAggregate> GetAsync(Guid id);
    Task AddAsync(TAggregate aggregate);
    Task AddAndPublishAsync(TAggregate aggregate);
    Task UpdateAsync(TAggregate aggregate);
    Task UpdateAndPublishAsync(TAggregate aggregate);
    Task DeleteAsync(TAggregate aggregate);
    Task DeleteAndPublishAsync(TAggregate aggregate);
}