namespace Tasheer.Shared.Configuration;

public class ElasticSearchOptions
{
    public static string ConfigurationKey => "ElasticSearch";

    public string Url { get; set; }
    public string Project { get; set; }
    public string Application { get; set; }
    public string IndexFormat { get; set; }
    public string ApiId { get; set; }
    public string ApiKey { get; set; }
    public string Environment { get; set; }
}