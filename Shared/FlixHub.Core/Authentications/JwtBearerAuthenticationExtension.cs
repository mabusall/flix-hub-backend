namespace FlixHub.Core.Authentications;

public static class JwtBearerAuthenticationExtension
{
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services,
                                                                JwtSecurityTokenOptions jwtSecurityToken)
    {
        var secret = jwtSecurityToken.Secret;
        var issuer = jwtSecurityToken.Issuer;
        var audience = jwtSecurityToken.Audience;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // Set to true in production
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Your tokens don't expire
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ClockSkew = TimeSpan.Zero // No tolerance for expiration time
                };

                // Add custom event to validate token exists in cache
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        // Get the cache provider from DI
                        var cacheProvider = context.HttpContext.RequestServices
                            .GetRequiredService<IMemoryCacheProvider>();

                        // Extract email from token claims
                        var emailClaim = context.Principal?.FindFirst(ClaimTypes.Email)?.Value;

                        if (string.IsNullOrWhiteSpace(emailClaim))
                        {
                            context.Fail("Email claim not found in token");
                            return;
                        }

                        // Check if token exists in cache
                        var cachedToken = await cacheProvider.GetAsync<string>(emailClaim, context.HttpContext.RequestAborted);

                        if (string.IsNullOrWhiteSpace(cachedToken))
                        {
                            // Token not found in cache (user has logged out)
                            context.Fail("Token has been revoked");
                            return;
                        }

                        // Get the actual token from the request
                        var requestToken = context.Request.Headers.Authorization
                            .FirstOrDefault()?.Replace("Bearer ", "");

                        // Verify the cached token matches the request token
                        if (cachedToken != requestToken)
                        {
                            context.Fail("Token mismatch");
                            return;
                        }
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}