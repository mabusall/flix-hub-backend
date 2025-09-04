namespace Tasheer.Core.WebApplicationExtension;

public static class HttpExtensions
{
    public static void MapPing(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/Ping", async context =>
        {
            var response = $"Ping on {DateTime.UtcNow}";
            context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(response);
        }).AllowAnonymous();
    }

    public static string GetAccessToken(this HttpContext httpContext)
        => httpContext.Request.Headers.Authorization.FirstOrDefault().Replace("Bearer ", "");
}