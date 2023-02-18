using Shared.Domain.Abstractions;
using System.Reflection;

namespace Shared.Implementations.RabbitMQ;

public interface IRabbitEventListener
{
    void Subscribe(Assembly assembly, Type type, string? queueName = null, string? routingKey = null);
    void Subscribe<TEvent>(Assembly assembly) where TEvent : IEvent;
    Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    void Publish(string message, string key);
}