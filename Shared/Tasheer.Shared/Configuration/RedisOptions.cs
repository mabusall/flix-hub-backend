namespace Tasheer.Shared.Configuration;

public class RedisOptions
{
    public static string ConfigurationKey => "Redis";

    public string Uri { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = false;
}
