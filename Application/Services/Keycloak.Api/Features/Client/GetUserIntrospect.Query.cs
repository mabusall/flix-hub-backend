namespace Keycloak.Api.Features.Client;

public record KeycloakClientGetUserIntrospectQuery
(
    string AccessToken
) : IQuery<KeycloakClientGetUserIntrospectResult>;

public record KeycloakClientGetUserIntrospectResult
{
    [
        JsonPropertyName("exp"),
        JsonConverter(typeof(UnixTimestampToDateTimeConverter))
    ]
    public DateTime Expiration { get; set; } // Expiration time in Unix timestamp

    [
        JsonPropertyName("iat"),
        JsonConverter(typeof(UnixTimestampToDateTimeConverter))
    ]
    public DateTime IssuedAt { get; set; } // Issued at time in Unix timestamp

    [JsonPropertyName("jti")]
    public string? Jti { get; set; } // JWT ID (unique identifier)

    [JsonPropertyName("iss")]
    public string? Issuer { get; set; } // Issuer URL

    [JsonPropertyName("sub")]
    public string? Subject { get; set; } // Subject (user identifier)

    [JsonPropertyName("typ")]
    public string? Type { get; set; } // Type (e.g., "Bearer")

    [JsonPropertyName("azp")]
    public string? AuthorizedParty { get; set; } // Authorized party (client ID)

    [JsonPropertyName("session_state")]
    public string? SessionState { get; set; } // The session state

    [JsonPropertyName("name")]
    public string? FullName { get; set; } // Full name of the user

    [JsonPropertyName("given_name")]
    public string? FirstName { get; set; } // Given name (first name)

    [JsonPropertyName("family_name")]
    public string? LastName { get; set; } // Family name (last name)

    [JsonPropertyName("preferred_username")]
    public string? PreferredUsername { get; set; } // Preferred username

    [JsonPropertyName("email")]
    public string? Email { get; set; } // User email

    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; } // Indicates if the email is verified

    [JsonPropertyName("scope")]
    public string? Scope { get; set; } // Scopes granted to the user (e.g., "openid profile email")

    [JsonPropertyName("sid")]
    public string? SessionId { get; set; } // Session ID (same as session_state)

    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; } // Client ID that the token was issued for

    [JsonPropertyName("username")]
    public string? Username { get; set; } // Username (also `preferred_username`)

    [JsonPropertyName("active")]
    public bool IsActive { get; set; } // Indicates if the token is still active
};