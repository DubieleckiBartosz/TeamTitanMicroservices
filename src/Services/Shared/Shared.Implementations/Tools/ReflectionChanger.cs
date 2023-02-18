namespace Shared.Implementations.Tools;

public static class ReflectionChanger
{
    public static void SetNewValue<T>(this T item, string propertyName, object value)
    {
        item?.GetType()
            .GetProperty(propertyName)?
            .SetValue(item, value);
    }
}