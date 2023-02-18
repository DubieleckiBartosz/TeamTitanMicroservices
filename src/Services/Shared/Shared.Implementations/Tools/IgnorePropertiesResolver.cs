using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace Shared.Implementations.Tools;

public class IgnorePropertiesResolver : DefaultContractResolver
{
    private readonly List<string> _ignoreProps;
    public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
    {
        this._ignoreProps = new List<string>(propNamesToIgnore);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (this._ignoreProps.Contains(property.PropertyName!))
        {
            property.ShouldSerialize = _ => false;
        }
        return property;
    }
}