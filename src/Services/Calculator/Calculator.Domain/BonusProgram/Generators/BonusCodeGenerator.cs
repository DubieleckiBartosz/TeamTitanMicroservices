using System.Text;

namespace Calculator.Domain.BonusProgram.Generators;

public class BonusCodeGenerator
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public static string GenerateBonusCode(string codeBasedOn, string codeFor)
    {
        var code = new StringBuilder();

        var random = new Random();

        var partCode = ClearStringToAlphanumeric(codeBasedOn);

        code.Append(codeFor.ToUpper() + "_");
        code.Append(partCode);

        for (var i = 0; i < 4; i++)
        {
            var index = random.Next(Characters.Length);
            code.Append(Characters[index]);
        }

        return code.ToString();
    }

    public static string ClearStringToAlphanumeric(string input)
    {
        var output = new StringBuilder();

        foreach (char c in input)
        {
            if (output.Length == 4)
            {
                break;
            }

            if (Characters.Contains(c))
            {
                output.Append(c);
            }
        }

        return output.ToString();
    }
}