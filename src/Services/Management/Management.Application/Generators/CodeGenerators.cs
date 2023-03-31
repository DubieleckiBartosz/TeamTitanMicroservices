using System.Security.Cryptography;
using System.Text;

namespace Management.Application.Generators;

public class CodeGenerators
{
    const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public static string CompanyCodeGenerate()
    {
        var random = new Random();
        var code = new string(Enumerable.Repeat(Chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return code;
    }

    public static string PersonCompanyCodeGenerate(string companyCode, string position)
    {
        if (string.IsNullOrEmpty(companyCode))
        {
            throw new ArgumentException("Company code cannot be null or empty.", nameof(companyCode));
        }
        
        if (string.IsNullOrEmpty(position))
        {
            throw new ArgumentException("Role cannot be null or empty.", nameof(position));
        }

        var basedOn = companyCode.Substring(0, 4);

        var ticks = DateTime.UtcNow.Ticks.ToString();
        
        var data = $"{basedOn}{ticks}";

        // Compute a SHA-256 hash of the data to generate a unique 64-character hex string.
        var bytes = Encoding.UTF8.GetBytes(data);
        var hashBytes = SHA256.Create().ComputeHash(bytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "");

        // Take the first 8 characters of the hash to get a 32-bit integer.
        var randomInt = int.Parse(hash.Substring(0, 6), System.Globalization.NumberStyles.HexNumber);
         
        var roleString = position.Substring(0, 3).ToUpper();
        var randomString = randomInt.ToString().Substring(0, 5).ToUpper();
        var employeeCode = $"{roleString}{basedOn}{randomString}"; //12 characters

        return employeeCode;
    }
}