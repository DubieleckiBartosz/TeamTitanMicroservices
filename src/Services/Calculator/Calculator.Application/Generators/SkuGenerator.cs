namespace Calculator.Application.Generators;

public static class SkuGenerator
{
    private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int Length = 8;

    private static readonly Random Random = new Random();

    public static string Generate()
    {
        var sku = new char[Length];

        for (var i = 0; i < Length; i++)
        {
            sku[i] = AllowedChars[Random.Next(AllowedChars.Length)];
        }

        return new string(sku);
    }
}