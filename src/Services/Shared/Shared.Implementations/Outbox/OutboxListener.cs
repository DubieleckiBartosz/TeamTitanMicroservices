using Newtonsoft.Json;
using Shared.Domain.Abstractions;
using Shared.Implementations.Logging;
using Shared.Implementations.Outbox.MongoOutbox;
using Shared.Implementations.Tools;

namespace Shared.Implementations.Outbox;

public class OutboxListener : IOutboxListener
{
    private readonly IOutboxStore _outboxStore;
    private readonly ILoggerManager<OutboxListener> _loggerManager;

    public OutboxListener(IOutboxStore outboxStore, ILoggerManager<OutboxListener> loggerManager)
    {
        _outboxStore = outboxStore ?? throw new ArgumentNullException(nameof(outboxStore));
        _loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
    }
    public async Task Commit(OutboxMessage message)
    {
        await _outboxStore.AddAsync(message);
    }

    public async Task Commit<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var type = @event?.GetType()?.AssemblyQualifiedName;

        if (type != null)
        {
            var outboxMessage = new OutboxMessage
            {
                Type = type,
                QueueKey = type.CreateAlternativeKey(),
                Data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            };

            await Commit(outboxMessage);
        }
        else
        {
            _loggerManager.LogError("Event is null.");
        }
    }
}