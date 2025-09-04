namespace Keycloak.Api.Features.Client;

public record KeycloakClientRefreshTokenCommand
(
    string RefreshToken
) : ICommand<KeycloakClientRefreshTokenResult>;

public record KeycloakClientRefreshTokenResult
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [
        JsonConverter(typeof(SecondsToDateTimeConverter)),
        JsonPropertyName("expires_in")
    ]
    public DateTime ExpiresIn { get; set; }

    [
        JsonConverter(typeof(SecondsToDateTimeConverter)),
        JsonPropertyName("refresh_expires_in")
    ]
    public DateTime RefreshExpiresIn { get; set; }
}