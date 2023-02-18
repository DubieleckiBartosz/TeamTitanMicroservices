using System.Net;
using MongoDB.Driver;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Mongo;

namespace Shared.Implementations.Outbox.MongoOutbox;

public class OutboxStore : IOutboxStore
{
    private readonly IMongoRepository<OutboxMessage> _mongoRepository;

    public OutboxStore(IMongoRepository<OutboxMessage> mongoRepository)
    {
        _mongoRepository = mongoRepository ?? throw new ArgumentNullException(nameof(mongoRepository));
    }

    public async Task AddAsync(OutboxMessage message)
    {
        await _mongoRepository.AddAsync(message);
    }

    public async Task<IEnumerable<Guid>> GetUnprocessedMessageIdsAsync()
    {
        var filter = Builders<OutboxMessage>.Filter.Where(_ => !_.Processed.HasValue);
        var cursor = await _mongoRepository.Collection.Find(filter).ToCursorAsync();

        var result = cursor
            .ToList()
            .Select(c => c.Id);

        return result;
    }

    public async Task SetMessageToProcessedAsync(Guid id)
    {
        var filter = Builders<OutboxMessage>.Filter.Where(_ => _.Id == id);
        var update = Builders<OutboxMessage>.Update.Set(_ => _.Processed, DateTime.UtcNow);

        var result = await _mongoRepository.Collection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0)
        {
            throw new TeamTitanApplicationException($"Did not modify message '{id}'", "Processing Message",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task DeleteAsync(IEnumerable<Guid> ids)
    {
        var filter = Builders<OutboxMessage>.Filter.In(_ => _.Id, ids);
        await _mongoRepository.Collection.DeleteManyAsync(filter);
    }

    public async Task<OutboxMessage?> GetMessageAsync(Guid id)
    {
        return await _mongoRepository.GetAsync(_ => _.Id == id);
    }
}