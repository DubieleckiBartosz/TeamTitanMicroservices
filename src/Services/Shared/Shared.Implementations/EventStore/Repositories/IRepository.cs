using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Shared.Implementations.EventStore.Repositories;

public interface IRepository<TAggregate> where TAggregate : Aggregate
{
    Task<TAggregate?> GetAggregateFromSnapshotAsync<TSnapshot>(Guid id) where TSnapshot : ISnapshot;
    Task<TAggregate> GetAsync(Guid id);
    Task AddAsync(TAggregate aggregate);
    Task UpdateWithSnapshotAsync<TSnapshot>(TAggregate aggregate);
    Task AddAndPublishAsync(TAggregate aggregate);
    Task UpdateAsync(TAggregate aggregate);
    Task UpdateAndPublishAsync(TAggregate aggregate);
    Task DeleteAsync(TAggregate aggregate);
    Task DeleteAndPublishAsync(TAggregate aggregate);
}