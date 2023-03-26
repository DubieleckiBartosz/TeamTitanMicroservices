namespace Management.Application.Generators;

public class CompanyCodeGenerator
{
    const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string Generate()
    { 
        var random = new Random();
        var code = new string(Enumerable.Repeat(Chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
         
        return code;
    }
}