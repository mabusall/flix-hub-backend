namespace Keycloak.Api.Features.Client;

public record KeycloakClientGetUserSessionsQuery
(
    string Email
) : IQuery<List<KeycloakClientGetUserSessionsResult>>;

public record KeycloakClientGetUserSessionsResult
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public Guid UserId { get; set; }
    public string? IpAddress { get; set; }
    [JsonConverter(typeof(UnixTimestampToDateTimeConverter))]
    public DateTime Start { get; set; }
    [JsonConverter(typeof(UnixTimestampToDateTimeConverter))]
    public DateTime LastAccess { get; set; }
};