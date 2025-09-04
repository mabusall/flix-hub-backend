namespace Mabusall.Shared.DataProtection;

public interface IEncryptionDataProtectionProvider
{
    void UseThisKey(string key);

    string Protect(string plaintext);

    string Unprotect(string protectedData);
}