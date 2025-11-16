namespace FlixHub.Shared.DataProtection;

public static class DataProtectionProviderExtention
{
    public static string PublicKey { get; private set; }

    public static string SaltKey { get; private set; }

    public static readonly char[] padding = ['='];

    public static void Initialize(string publicKey, string saltKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(publicKey, nameof(publicKey));
        ArgumentException.ThrowIfNullOrWhiteSpace(saltKey, nameof(saltKey));

        PublicKey = publicKey;
        SaltKey = saltKey;
    }

    public static string Encrypt(this string textToEncrypt)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(PublicKey, nameof(PublicKey));
            ArgumentException.ThrowIfNullOrWhiteSpace(SaltKey, nameof(SaltKey));

            byte[] secretkeyByte = Encoding.UTF8.GetBytes(SaltKey);
            byte[] publickeybyte = Encoding.UTF8.GetBytes(PublicKey);
            byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);

            using Aes aes = Aes.Create();
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
            cs.Write(inputbyteArray, 0, inputbyteArray.Length);
            cs.FlushFinalBlock();

            var res = Convert.ToBase64String(ms.ToArray())
                .TrimEnd(padding)
                .Replace('+', '-')
                .Replace('/', '_');

            return res;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static byte[] Encrypt(this byte[] data)
    {
        if (data is null || data.Length <= 0) return null;

        var key = PublicKey.Get256ShaHash();
        var salt = SaltKey.Get128MD5Hash();

        return new AesEncryptionProvider().Encrypt(data, key, salt);
    }

    public static string EncryptDB(this string textToEncrypt)
    {
        try
        {
            return textToEncrypt.Encrypt();
        }
        catch
        {
            return textToEncrypt;
        }
    }

    public static string Decrypt(this string textToDecrypt)
    {
        try
        {
            // Print the public and salt key to the console.
            ArgumentException.ThrowIfNullOrWhiteSpace(PublicKey, nameof(PublicKey));
            ArgumentException.ThrowIfNullOrWhiteSpace(SaltKey, nameof(SaltKey));
            ArgumentException.ThrowIfNullOrWhiteSpace(textToDecrypt, nameof(textToDecrypt));

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

    public static byte[] Decrypt(this byte[] data)
    {
        if (data is null || data.Length <= 0) return null;

        var key = PublicKey.Get256ShaHash();
        var salt = SaltKey.Get128MD5Hash();

        return new AesEncryptionProvider().Decrypt(data, key, salt);
    }

    public static string DecryptDB(this string textToDecrypt)
    {
        try
        {
            return textToDecrypt.Decrypt();
        }
        catch
        {
            return textToDecrypt;
        }
    }
}