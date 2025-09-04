namespace Tasheer.Core.Services;

public class ValidTokenFilter : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext endpointFilterInvocationContext,
                                         EndpointFilterDelegate next)
    {
        var httpContext = endpointFilterInvocationContext.HttpContext;
        var appSettingsKeyManagement = httpContext.RequestServices.GetRequiredService<IAppSettingsKeyManagement>();
        var httpClientFactory = httpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
        var applicationLifetime = httpContext.RequestServices.GetRequiredService<IManagedCancellationToken>();
        var accessToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        // Check if the Authorization header is present        
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            throw new UnauthorizedException("Authorization token not found");
        }

        #region [ check token validity ]

        var cancellationToken = applicationLifetime.Token;
        var options = appSettingsKeyManagement.KeycloakOptions;

        var clientRealm = options!.Realms["Client"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.UserInfo}", clientRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        using var httpClient = httpClientFactory.CreateClient();

        httpClient.BaseAddress = new Uri(url);
        httpClient.AttachBearerAuthentication(accessToken);
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            throw new UnauthorizedException("Token expired");
        }

        #endregion

        // Proceed to the next filter or handler if ValidToken are satisfied
        return await next(endpointFilterInvocationContext);
    }
}