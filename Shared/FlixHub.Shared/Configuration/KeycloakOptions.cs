namespace FlixHub.Shared.Configuration;

public class KeycloakOptions
{
    public static string ConfigurationKey => "Keycloak";

    public Dictionary<string, Realm> Realms { get; set; }
    public EndPoints EndPoints { get; set; }
}

public class Realm
{
    public string Name { get; set; }
    public string Client { get; set; }
    public Guid ClientId { get; set; }
    public string Scopes { get; set; }
    public string Secret { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class EndPoints
{
    public string BaseAddress { get; set; }
    public string Metadata { get; set; }
    public string TokenExchange { get; set; }
    public string CreateUser { get; set; }
    public string LogoutUser { get; set; }
    public string UserInfo { get; set; }
    public string Introspect { get; set; }
}