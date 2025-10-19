namespace FlixHub.Core.Authorization;

public class HangfireDashboardAllowAllAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Allow access to everyone for now (change in production)
        return true;
    }
}