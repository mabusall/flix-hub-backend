namespace FlixHub.Shared.Configuration;

public class RabbitMqOptions
{
    public static string ConfigurationKey => "MessageBus";

    public string Uri { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Amqp { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool AutoStart { get; set; }
    public bool AutoDelete { get; set; }
    public bool DisableDeliveryService { get; set; }
    public bool DisableInboxCleanupService { get; set; }
    public int PrefetchCount { get; set; }
    public int ConcurrentMessageLimit { get; set; }
    public int RetryCount { get; set; }
    public TimeSpan Interval { get; set; }
    public string DbConnection { get; set; }
    public Dictionary<string, ConsumerEndpoint> Consumers { get; set; } = [];
}

public class ConsumerEndpoint
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Consumer { get; set; }
    public bool UseDatabase { get; set; }
    public int PrefetchCount { get; set; }
    public int ConcurrentMessageLimit { get; set; }
    public int RetryCount { get; set; }
    public TimeSpan Interval { get; set; }
}