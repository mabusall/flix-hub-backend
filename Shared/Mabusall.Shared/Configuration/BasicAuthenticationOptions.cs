namespace Mabusall.Shared.Configuration;

public class BasicAuthenticationOptions
{
    public static string ConfigurationKey => "BasicAuthentication";

    public string UserName { get; set; }
    public string Password { get; set; }
}