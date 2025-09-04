namespace Tasheer.Core.Authentications;

public static class KeycloackAuthenticationExtension
{
    public static IServiceCollection AddKeyclockAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var keycloakOptions = configuration
            .GetSection(KeycloakOptions.ConfigurationKey)
            .Get<KeycloakOptions>();
        var clientRealm = keycloakOptions.Realms["Client"];
        var metaAddr = string.Format(keycloakOptions.EndPoints.Metadata, clientRealm.Name);

        services
            .AddAuthorization()

            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
            {
                options.MetadataAddress = $"{keycloakOptions.EndPoints.BaseAddress}{metaAddr}";
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        context.NoResult();
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var idenity = context.Principal.Identities.First();
                        var claims = GetAllCustomClaims(idenity.Claims, clientRealm);

                        idenity.AddClaims(claims);

                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }

    private static List<Claim> GetAllCustomClaims(IEnumerable<Claim> allClaims, Realm realm)
    {
        var resourceAccessClaim = allClaims.FirstOrDefault(w => w.Type == "resource_access");
        var claims = new List<Claim>();

        if (resourceAccessClaim is not null)
        {
            // Deserialize the resource_access JSON into a dictionary
            var resourceAccessJson = resourceAccessClaim.Value;
            var resourceAccess = JsonSerializerHandler.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(resourceAccessJson);

            // Extract the roles for 'spp-cli' client
            if (resourceAccess is not null && resourceAccess.TryGetValue(realm.Client, out Dictionary<string, List<string>> value))
            {
                var roles = value["roles"];

                // Enumerate the roles
                foreach (var role in roles)
                {
                    claims.Add(new(ClaimTypes.Role, role));
                }
            }
        }

        return claims;
    }
}