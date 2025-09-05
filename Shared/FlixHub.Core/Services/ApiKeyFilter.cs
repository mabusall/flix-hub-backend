namespace FlixHub.Core.Services;

public class ApiKeyFilter : IEndpointFilter
{
    // You can choose a different header name.
    private readonly string _apiKeyHeaderName = "X-Api-Key";

    public static string ApiKey { get; set; }

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext endpointFilterInvocationContext,
                                               EndpointFilterDelegate next)
    {
        var httpContext = endpointFilterInvocationContext.HttpContext;

        // Read the API key from configuration
        if (!httpContext.Request.Headers.TryGetValue(_apiKeyHeaderName, out var apiKey) ||
            !apiKey.Equals(ApiKey))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            throw new InvalidApiKeyException("Invalid API Key");
        }

        // Proceed to the next filter or handler if ApiKey are satisfied
        return await next(endpointFilterInvocationContext);
    }
}