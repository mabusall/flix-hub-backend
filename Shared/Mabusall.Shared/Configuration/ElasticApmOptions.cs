namespace Mabusall.Shared.Configuration;

public class ElasticApmOptions
{
    public static string ConfigurationKey => "ElasticApm";

    public string ServerUrl { get; set; }
    public string ServiceName { get; set; }
    public string Environment { get; set; }
    public bool IsEnabled { get; set; }
}