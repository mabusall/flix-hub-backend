namespace FlixHub.Shared.Configuration;

public record IntegrationApisOptions
{
    public static string ConfigurationKey => "IntegrationApis";

    public Dictionary<string, IntegrationApi> Apis { get; set; } = [];
}

public record IntegrationApi
{
    public string BaseUrl { get; set; }
    public string Token { get; set; }
    public string ResourcesUrl { get; set; }
}