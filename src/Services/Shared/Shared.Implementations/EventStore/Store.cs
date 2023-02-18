using Dapper;
using Microsoft.Extensions.Options;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;
using System.Data;

namespace Shared.Implementations.EventStore;

public class Store : IStore
{
    private readonly DapperConnection<Store> _connection;
    public Store(IOptions<EventStoreOptions> eventStoreOptions, ILoggerManager<Store> logger)
    {
        if (eventStoreOptions == null)
        {
            throw new ArgumentNullException(nameof(eventStoreOptions));
        }

        var options = eventStoreOptions.Value;
        _connection = new DapperConnection<Store>(options.EventStoreConnection, logger);
    }

    public async Task AddAsync(StreamState stream, long? expectedVersion)
    {
        var param = new DynamicParameters();

        param.Add("@id", stream.Id);
        param.Add("@data", stream.StreamData);
        param.Add("@type", stream.EventType);
        param.Add("@streamId", stream.StreamId);
        param.Add("@streamType", stream.StreamType);
        param.Add("@expectedStreamVersion", expectedVersion);
        param.Add("@resultVersion", -1, DbType.Int32, ParameterDirection.Output);

        await _connection.WithConnection(_ =>
            _.ExecuteAsync("event_AppendEvent_IU", param, commandType: CommandType.StoredProcedure));

        var resultVersionSuccess = param.Get<int>("@resultVersion");
        if (resultVersionSuccess == 0)
        {
            throw new EventException("Expected version did not match the stream version!", "Bad Stream Version");
        }
    }

    public async Task<IReadOnlyList<StreamState>?> GetEventsAsync(Guid aggregateId, long? version = null, DateTime? createdUtc = null)
    {
        var param = new DynamicParameters();

        param.Add("@streamVersion", version);
        param.Add("@atTimestamp", createdUtc);
        param.Add("@streamId", aggregateId);

        var result = (await _connection.WithConnection(_ =>
            _.QueryAsync<StreamState>("event_GetBySearch_S", param, commandType: CommandType.StoredProcedure)))?.ToList();

        return result?.AsReadOnly();
    }
}