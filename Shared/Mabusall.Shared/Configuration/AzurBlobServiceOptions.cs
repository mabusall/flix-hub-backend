namespace Mabusall.Shared.Configuration;

public record AzurBlobServiceOptions
{
    public static string ConfigurationKey => "AzurBlobService";

    public string Uri { get; set; }
    public string CdnUri { get; set; }
    public string Token { get; set; }
}
