namespace Mabusall.Shared.Configuration;

public class LogsOptions
{
    public required string Project { get; set; }
    public required string Application { get; set; }
    public required string Environment { get; set; }
    public required FileOptions File { get; set; }
    public required ConsoleOptions Console { get; set; }
    public required DebugOptions Debug { get; set; }
    public required ElasticsearchOptions Elasticsearch { get; set; }
}

public class ConsoleOptions
{
    public required bool IsEnabled { get; set; }
}

public class DebugOptions
{
    public required bool IsEnabled { get; set; }
}

public class FileOptions
{
    public required bool IsEnabled { get; set; }
    public required string Path { get; set; }
}

public class ElasticsearchOptions
{
    public required bool IsEnabled { get; set; }
    public required string Url { get; set; }
    public required string IndexFormat { get; set; }
    public required string ApiId { get; set; }
    public required string ApiKey { get; set; }
}