using System.Security.Cryptography;
using System.Text;

namespace Shared.Implementations.Tools;

public static class SecurityTools
{
    public static string Encrypt(this string value, string encryptionKey)
    {
        try
        {
            var iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aes.IV = iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(value);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string Decrypt(this string encryptedValue, string encryptionKey)
    {
        try
        {
            var iv = new byte[16];
            var buffer = Convert.FromBase64String(encryptedValue);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = iv;
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader((Stream)cryptoStream);
            return streamReader.ReadToEnd();
        }
        catch
        {
            return string.Empty;
        }
    }
}