using System.Security.Cryptography;
using System.Text;

namespace Management.Application.Generators;

public class EmployeeCodeGenerator
{
    public static string Generate(string companyCode)
    {
        if (string.IsNullOrEmpty(companyCode))
        {
            throw new ArgumentException("Company code cannot be null or empty.", nameof(companyCode));
        }

        var basedOn = companyCode.Substring(0, 4);

        var ticks = DateTime.UtcNow.Ticks.ToString(); 
        var data = $"{basedOn}-{ticks}";

        // Compute a SHA-256 hash of the data to generate a unique 64-character hex string.
        var bytes = Encoding.UTF8.GetBytes(data);
        var hashBytes = SHA256.Create().ComputeHash(bytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "");

        // Take the first 8 characters of the hash to get a 32-bit integer.
        var randomInt = int.Parse(hash.Substring(0, 8), System.Globalization.NumberStyles.HexNumber);
         
        var randomString = randomInt.ToString("D7"); 
        var employeeCode = $"EMP{basedOn}-{randomString}";

        return employeeCode;
    }
}