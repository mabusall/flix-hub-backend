namespace FlixHub.Shared.Configuration;

public class JwtSecurityTokenOptions
{
    public static string ConfigurationKey => "JwtSecurityToken";

    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
