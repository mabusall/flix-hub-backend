using Mabusall.Tools.Security.WinApp.Extension;
using Microsoft.Extensions.Configuration;

namespace Mabusall.Tools.Security.WinApp;

internal class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Create a ConfigurationBuilder
        var configuration = new ConfigurationBuilder()
            .AddIniFile("default-config.ini", false, true)
            .Build();

        // Access the secrets
        string publicId = configuration["TasheerKeys:VaultKey1"];
        string secretId = configuration["TasheerKeys:VaultKey2"];

        DataProtectionProviderExtention.Initialize(publicId, secretId);

        Application.Run(new frmMain());
    }
}