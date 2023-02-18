using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Implementations.Logging;
using Shared.Implementations.Outbox.MongoOutbox;
using Shared.Implementations.RabbitMQ;

namespace Shared.Implementations.Outbox;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILoggerManager<OutboxProcessor> _loggerManager;
    private readonly MongoOutboxOptions _outboxOptions;

    public OutboxProcessor(IServiceScopeFactory serviceScopeFactory, IOptions<MongoOutboxOptions> outboxOptions,
            ILoggerManager<OutboxProcessor> loggerManager)
    {
        if (outboxOptions == null)
        {
            throw new ArgumentNullException(nameof(outboxOptions));
        }

        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
        _outboxOptions = outboxOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Process();

            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }
    }

    private async Task Process()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IOutboxStore>();
        var messageIds = await store.GetUnprocessedMessageIdsAsync();
        var publishedMessageIds = new List<Guid>();
        try
        {
            foreach (var messageId in messageIds)
            {
                _loggerManager.LogInformation("---------- New Process Started ----------");

                _loggerManager.LogInformation($"---------- Process Message Id: {messageId} ----------");

                var message = await store.GetMessageAsync(messageId);
                if (message == null || message.Processed.HasValue)
                {
                    _loggerManager.LogWarning(new
                    {
                        Message = "---------- Stop Process !!! ----------",
                        MessageIsNull = message == null ? true : false,
                        IsProcessed = message?.Processed.HasValue
                    });

                    continue;
                }

                var serviceRabbitListener = scope.ServiceProvider.GetRequiredService<IRabbitEventListener>();
                serviceRabbitListener.Publish(message.Data, message.QueueKey ?? message.Type);
                await store.SetMessageToProcessedAsync(message.Id);

                _loggerManager.LogInformation($"---------- Message Processed: {messageId} ----------");

                publishedMessageIds.Add(message.Id);
            }

            if (publishedMessageIds.Any())
            {
                _loggerManager.LogInformation(
                    $"---------- Message to remove: {string.Join(", ", publishedMessageIds)} ----------");
            }
        }
        finally
        {
            if (_outboxOptions.DeleteAfter)
            {
                await store.DeleteAsync(publishedMessageIds);
            }
        }
    }
}