using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore; 

namespace Shared.Implementations.Tools;

public static class BaseSerialize
{
    public static byte[]? DataSerialize<T>(this T data)
    {
        if (data == null) return null;

        var json = JsonConvert.SerializeObject(data);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }

    public static T? Deserialize<T>(this byte[]? bytes)
    {
        if (bytes == null) return default;
        var result = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(result)!;
    }

    public static IEvent? DeserializeEvent(this string eventData)
    {
        var data = JsonConvert.DeserializeObject<IEvent>(eventData, new JsonSerializerSettings
        {
            ContractResolver = new PrivateResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            TypeNameHandling = TypeNameHandling.All
        });

        return data;
    }
     
    public static TResponse? DeserializeSnapshot<TResponse>(this string snapshotData) where TResponse : ISnapshot
    {
        var data = JsonConvert.DeserializeObject<TResponse>(snapshotData, new JsonSerializerSettings
        {
            ContractResolver = new PrivateResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            TypeNameHandling = TypeNameHandling.All
        });

        return data;
    }

    public static IEvent? DeserializeQueueEvent(this string queueData, Assembly assembly, string queueKey)
    {
        var type = assembly.GetTypes().Where(_ =>
        {
            var attribute = Attribute.GetCustomAttribute(_, typeof(EventQueueAttribute));
            if (attribute == null)
            {
                return false;
            }

            if (!_.GetInterfaces().Contains(typeof(IEvent)))
            {
                return false;
            }

            var attr = (EventQueueAttribute)attribute;
            var valueRoutingKey = attr?.RoutingKey;
            return valueRoutingKey != null && valueRoutingKey.Equals(queueKey);

        }).Single();

        var data = JsonConvert.DeserializeObject(queueData, type, new JsonSerializerSettings
        {
            ContractResolver = new PrivateResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        });

        return data as IEvent;
    }
}