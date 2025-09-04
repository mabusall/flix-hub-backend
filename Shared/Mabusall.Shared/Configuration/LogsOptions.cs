namespace Tasheer.Shared.Configuration;

public class LogsOptions
{
    required public string Project { get; set; }
    required public string Application { get; set; }
    required public string Environment { get; set; }
    required public FileOptions File { get; set; }
    required public ConsoleOptions Console { get; set; }
    required public DebugOptions Debug { get; set; }
    required public ElasticsearchOptions Elasticsearch { get; set; }
}

public class ConsoleOptions
{
    required public bool IsEnabled { get; set; }
}

public class DebugOptions
{
    required public bool IsEnabled { get; set; }
}

public class FileOptions
{
    required public bool IsEnabled { get; set; }
    required public string Path { get; set; }
}

public class ElasticsearchOptions
{
    required public bool IsEnabled { get; set; }
    required public string Url { get; set; }
    required public string IndexFormat { get; set; }
    required public string ApiId { get; set; }
    required public string ApiKey { get; set; }
}