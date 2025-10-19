namespace FlixHub.Core.Api.Features.SystemUsers;

public class GetListSystemUserQuery
    : PagingBase, IQuery<PaginatedList<SystemUserDto>>
{
    public Guid? Uuid { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsVerified { get; set; }
    public bool? IncludePreferences { get; set; }
    public bool? IncludeWatchlist { get; set; }

}