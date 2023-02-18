using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;
using Shared.Implementations.Logging;
using Shared.Implementations.Tools;


namespace Shared.Implementations.RabbitMQ;

public class RabbitEventListener : IRabbitEventListener
{
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
    {
        ContractResolver = new PrivateResolver(),
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        TypeNameHandling = TypeNameHandling.All
    };

    private const string ExchangeName = "team_titan_event_bus";
    private readonly IRabbitBase _rabbitBase;
    private readonly IServiceScopeFactory _serviceFactory;
    private readonly ILoggerManager<RabbitEventListener> _loggerManager;

    public RabbitEventListener(IRabbitBase rabbitBase, IServiceScopeFactory serviceFactory,
        ILoggerManager<RabbitEventListener> loggerManager)
    {
        _rabbitBase = rabbitBase ?? throw new ArgumentNullException(nameof(rabbitBase));
        _serviceFactory = serviceFactory;
        _loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
    }

    public void Subscribe(Assembly assembly, Type type, string? queueName = null, string? routingKey = null)
    {
        var model = _rabbitBase.GetOrCreateNewModelWhenItIsClosed();

        var args = _rabbitBase.CreateDeadLetterQueue(model).GetAwaiter().GetResult();

        var name = queueName ?? AppDomain.CurrentDomain.FriendlyName.Trim().Trim('_') + "_" + type.Name;
        var key = routingKey ?? CreateRoutingKey(type);
        _rabbitBase.CreateConsumer(model, ExchangeName, name, key, args);

        var mainConsumer = new AsyncEventingBasicConsumer(model);

        mainConsumer.Received += async (m, ea) =>
        {
            try
            {

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _loggerManager.LogInformation(new
                {
                    MessageId = ea.BasicProperties.MessageId,
                    Message = $"Received a message: {message}",
                });

                var data = message.DeserializeQueueEvent(assembly, key);

                using var scope = _serviceFactory.CreateScope();
                var eventBus = scope.ServiceProvider.GetService<IEventBus>();
                if (data != null)
                {
                    await eventBus?.PublishLocalAsync(data)!;
                }

                model.BasicAck(ea.DeliveryTag, false);

            }
            catch
            {
                model.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        model.BasicConsume(
            queue: name,
            autoAck: false,
            consumer: mainConsumer);
    }

    public void Subscribe<TEvent>(Assembly assembly) where TEvent : IEvent
    {
        Subscribe(assembly, typeof(TEvent));
    }

    public Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var model = _rabbitBase.GetOrCreateNewModelWhenItIsClosed();
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

        _rabbitBase.CreatePublisher(model, ExchangeName, CreateRoutingKey(typeof(TEvent)), body);

        return Task.CompletedTask;
    }

    public void Publish(string message, string key)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message), "Event message can not be null.");
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key), "Event type can not be null.");
        }

        var channel = _rabbitBase.GetOrCreateNewModelWhenItIsClosed();

        _rabbitBase.CreatePublisher(channel, ExchangeName, key, Encoding.UTF8.GetBytes(message));
    }

    private string CreateRoutingKey(Type type)
    {
        var name = type.Name.ToLower();

        if (type.GetInterfaces().Contains(typeof(IEvent)))
        {
            name += "_event";
        }

        return name;
    }
}