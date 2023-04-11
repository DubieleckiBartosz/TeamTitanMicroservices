using System.Text;

namespace Shared.Implementations.Tools;

public static class QueueKeyCreator
{
    public static string CreateAlternativeKey(this string typeName)
    {
        var indexTypeName = typeName.IndexOf(',');
        var baseTypeName = typeName.Substring(0, indexTypeName) + "_key";
        var indexKey = baseTypeName.LastIndexOf('.');
        var key = baseTypeName.Substring(indexKey + 1);

        var builder = new StringBuilder();

        foreach (var charItem in key)
        {
            if (char.IsUpper(charItem))
            {
                if (builder.Length > 0)
                {
                    builder.Append('_');
                }

                builder.Append(char.ToLower(charItem));
            }
            else
            {
                builder.Append(charItem);
            }
        }

        var responseKey = builder.ToString();

        return responseKey;
    }
}