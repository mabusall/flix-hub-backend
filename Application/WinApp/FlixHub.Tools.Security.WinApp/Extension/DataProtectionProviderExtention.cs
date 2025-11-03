using System.Security.Cryptography;
using System.Text;

namespace FlixHub.Tools.Security.WinApp.Extension;

public static class DataProtectionProviderExtention
{
    public static string PublicKey { get; set; }
    public static string SaltKey { get; set; }

    static readonly char[] Padding = { '=' };

    public static void Initialize(string publicKey, string saltKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new ArgumentException($"'{nameof(publicKey)}' cannot be null or whitespace.", nameof(publicKey));
        }

        if (string.IsNullOrWhiteSpace(saltKey))
        {
            throw new ArgumentException($"'{nameof(saltKey)}' cannot be null or whitespace.", nameof(saltKey));
        }

        PublicKey = publicKey;
        SaltKey = saltKey;
    }

    public static string Encrypt(this string textToEncrypt)
    {
        try
        {
            byte[] secretkeyByte = Encoding.UTF8.GetBytes(SaltKey);
            byte[] publickeybyte = Encoding.UTF8.GetBytes(PublicKey);
            byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);

            using Aes aes = Aes.Create();
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
            cs.Write(inputbyteArray, 0, inputbyteArray.Length);
            cs.FlushFinalBlock();

            var res = Convert.ToBase64String(ms.ToArray())
                             .TrimEnd(Padding)
                             .Replace('+', '-')
                             .Replace('/', '_');
            return res;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static string Decrypt(this string textToDecrypt)
    {
        try
        {
            string incoming = textToDecrypt.Replace('_', '/').Replace('-', '+');
            switch (textToDecrypt.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }

            byte[] secretkeyByte = Encoding.UTF8.GetBytes(SaltKey);
            byte[] publickeybyte = Encoding.UTF8.GetBytes(PublicKey);
            byte[] inputbyteArray = new byte[incoming.Length];

            inputbyteArray = Convert.FromBase64String(incoming);

            using Aes aes = Aes.Create();
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateDecryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
            cs.Write(inputbyteArray, 0, inputbyteArray.Length);
            cs.FlushFinalBlock();
            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        catch (Exception)
        {
            throw;
        }
    }
}
