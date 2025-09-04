namespace Keycloak.Api.Features.Client;

public record KeycloakClientLoginCommand
(
    string Email,
    string Password
) : ICommand<KeycloakClientLoginResult>;

public record KeycloakClientLoginResult
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

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

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("session_state")]
    public string? SessionState { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
}