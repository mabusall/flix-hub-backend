namespace FlixHub.Shared.Configuration;

public class DailySyncRequestsOptions
{
    public static string ConfigurationKey => "DailySyncRequests";

    public int Limit { get; set; }
    public int MovieQuota { get; set; }
    public int SeriesQuota { get; set; }
}