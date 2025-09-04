namespace Mabusall.Shared.Configuration;

public interface IAppSettingsKeyManagement
{
    ElasticApmOptions ElasticApmOptions { get; }
    ElasticSearchOptions ElasticSearchOptions { get; }
    HangfireOptions HangfireOptions { get; }
    KeycloakOptions KeycloakOptions { get; }
    RabbitMqOptions RabbitMqOptions { get; }
    RedisOptions RedisOptions { get; }
    SmtpEmailOptions SmtpEmailOptions { get; }
    BasicAuthenticationOptions BasicAuthenticationOptions { get; }
    AzurBlobServiceOptions AzurBlobServiceOptions { get; }
    RateLimitOptions RateLimitOptions { get; }
    Dictionary<string, bool> AppFeatures { get; }
    FirebaseOptions FirebaseOptions { get; }
}

public class AppSettingsKeyManagement(ElasticApmOptions elasticApmOptions,
                                      ElasticSearchOptions elasticSearchOptions,
                                      HangfireOptions hangfireOptions,
                                      KeycloakOptions keycloakOptions,
                                      RabbitMqOptions rabbitMqOptions,
                                      RedisOptions redisOptions,
                                      SmtpEmailOptions smtpEmailOptions,
                                      BasicAuthenticationOptions basicAuthenticationOptions,
                                      AzurBlobServiceOptions azurBlobServiceOptions,
                                      RateLimitOptions rateLimitOptions,
                                      Dictionary<string, bool> appFeatures,
                                      FirebaseOptions firebaseOptions)
    : IAppSettingsKeyManagement
{
    public ElasticApmOptions ElasticApmOptions { get; } = elasticApmOptions;

    public ElasticSearchOptions ElasticSearchOptions { get; } = elasticSearchOptions;

    public HangfireOptions HangfireOptions { get; } = hangfireOptions;

    public KeycloakOptions KeycloakOptions { get; } = keycloakOptions;

    public RabbitMqOptions RabbitMqOptions { get; } = rabbitMqOptions;

    public RedisOptions RedisOptions { get; } = redisOptions;

    public SmtpEmailOptions SmtpEmailOptions { get; } = smtpEmailOptions;

    public BasicAuthenticationOptions BasicAuthenticationOptions { get; } = basicAuthenticationOptions;

    public AzurBlobServiceOptions AzurBlobServiceOptions { get; } = azurBlobServiceOptions;

    public RateLimitOptions RateLimitOptions { get; } = rateLimitOptions;

    public Dictionary<string, bool> AppFeatures { get; } = appFeatures;

    public FirebaseOptions FirebaseOptions { get; } = firebaseOptions;
}