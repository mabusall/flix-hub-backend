namespace FlixHub.Shared.DataProtection;

public class EncryptionDataProtectionProvider(IDataProtectionProvider dataProtectionProvider) : IEncryptionDataProtectionProvider
{
    private IDataProtector _protector = dataProtectionProvider.CreateProtector("5etPr1v@teKey");

    public void UseThisKey(string key)
    {
        _protector = dataProtectionProvider.CreateProtector(key);
    }

    public string Protect(string plaintext)
    {
        if (string.IsNullOrEmpty(plaintext))
            return plaintext;

        return _protector.Protect(plaintext);
    }
    public string Unprotect(string protectedData)
    {
        if (string.IsNullOrEmpty(protectedData))
            return protectedData;

        return _protector.Unprotect(protectedData);
    }
}