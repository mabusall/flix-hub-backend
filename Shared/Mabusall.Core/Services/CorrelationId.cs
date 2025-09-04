namespace Tasheer.Core.Services;

public static class CorrelationIdExtesnsion
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        => app.Use(async (ctx, next) =>
        {
            if (!ctx.Request.Headers.TryGetValue(CorrelationIdService.CorrelationIdKey, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString("N");
            }

            ctx.Items[CorrelationIdService.CorrelationIdKey] = correlationId.ToString();
            using (Serilog.Context.LogContext.PushProperty(CorrelationIdService.CorrelationIdKey, correlationId))
            {
                await next();
            }
        });

    public static string GetCorrelationId(this HttpContext context)
        => context?.Items[CorrelationIdService.CorrelationIdKey] as string;

    public static void AddCorrelationId(this HttpRequestHeaders headers, string correlationId)
        => headers.TryAddWithoutValidation(CorrelationIdService.CorrelationIdKey, correlationId);
}