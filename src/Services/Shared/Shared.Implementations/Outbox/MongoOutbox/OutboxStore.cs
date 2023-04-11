using System.Net;
using MongoDB.Driver;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Mongo;

namespace Shared.Implementations.Outbox.MongoOutbox;

public class OutboxStore : IOutboxStore
{
    private const string CollectionName = "Messages";
    private readonly IMongoRepository<OutboxMessage> _mongoRepository;
    private readonly IMongoCollection<OutboxMessage> _collection;
    public OutboxStore(IMongoRepository<OutboxMessage> mongoRepository)
    {
        _mongoRepository = mongoRepository ?? throw new ArgumentNullException(nameof(mongoRepository));
        _collection = _mongoRepository.GetCollection(CollectionName);
    }

    public async Task AddAsync(OutboxMessage message)
    { 
        await _mongoRepository.AddAsync(message, _collection);
    }

    public async Task<IEnumerable<Guid>> GetUnprocessedMessageIdsAsync()
    {
        var filter = Builders<OutboxMessage>.Filter.Where(_ => _.IsProcessed == false);
        var cursor = await _collection.Find(filter).ToCursorAsync();

        var result = cursor
            .ToList()
            .Select(c => c.Id);

        return result;
    }

    public async Task SetMessageToProcessedAsync(Guid id)
    {
        var filter = Builders<OutboxMessage>.Filter.Where(_ => _.Id == id);
        var update = Builders<OutboxMessage>.Update
            .Set(_ => _.Processed, DateTime.UtcNow)
            .Set(_ => _.IsProcessed, true);

        var result = await _collection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0)
        {
            throw new TeamTitanApplicationException($"Did not modify message '{id}'", "Processing Message",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task DeleteAsync(IEnumerable<Guid> ids)
    {
        var filter = Builders<OutboxMessage>.Filter.In(_ => _.Id, ids);
        await _collection.DeleteManyAsync(filter);
    }

    public async Task<OutboxMessage?> GetMessageAsync(Guid id)
    {
        return await _mongoRepository.GetAsync(_ => _.Id == id, _collection);
    }
}