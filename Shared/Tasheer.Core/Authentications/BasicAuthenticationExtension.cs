namespace Tasheer.Core.Authentications;

public static class BasicAuthenticationExtension
{
    public static IServiceCollection AddBasicAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthorization()

            .AddAuthentication("BasicAuthentication")

            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

        return services;
    }
}

public class BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                        ILoggerFactory logger,
                                        IAppSettingsKeyManagement appSettingsKeyManagement,
                                        UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues value))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header missing"));
        }

        try
        {
            var basicAuthenticationOptions = appSettingsKeyManagement.BasicAuthenticationOptions;

            var authHeader = value.ToString();
            var authHeaderValue = authHeader.Split(' ')[1];
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue)).Split(':');

            var username = credentials[0];
            var password = credentials[1];

            if (username == basicAuthenticationOptions.UserName &&
                password == basicAuthenticationOptions.Password)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
            }
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header format"));
        }
    }
}