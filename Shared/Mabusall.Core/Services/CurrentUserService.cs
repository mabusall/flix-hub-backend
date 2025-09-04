namespace Tasheer.Core.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public ClaimsPrincipal User => httpContextAccessor?.HttpContext?.User;

    public string UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string UserName => User?.Identity?.Name;
}