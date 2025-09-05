namespace FlixHub.Shared.Configuration;

public record HangfireOptions
{
    public static string ConfigurationKey => "Hangfire";

    public bool IsEnabled { get; set; }
    public string DbConnection { get; set; }
    public string SchemaName { get; set; }
    public Dictionary<string, HangfireJob> Tasks { get; set; } = [];
}

public record HangfireJob
{
    public string Id { get; set; }
    public string Handler { get; set; }
    public bool IsEnabled { get; set; }
    public bool AutoStart { get; set; }
    public TimeSpan Schedule { get; set; }
}