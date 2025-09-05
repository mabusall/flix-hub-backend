namespace FlixHub.Core.Services;

public class CorrelationIdService(IHttpContextAccessor httpContextAccessor) : ICorrelationIdService
{
    public static string CorrelationIdKey => "X-CORRELATION-ID";

    public string Get()
    {
        try
        {
            return httpContextAccessor.HttpContext?.Items[CorrelationIdKey] as string;
        }
        catch { return string.Empty; }
    }
}
