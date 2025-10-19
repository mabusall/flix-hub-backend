namespace FlixHub.Core.Api.Features.SystemUsers;

internal class GetListSystemUserCommandHandler(IFlixHubDbUnitOfWork uofContext)
    : IQueryHandler<GetListSystemUserQuery, PaginatedList<SystemUserDto>>
{
    public async Task<PaginatedList<SystemUserDto>> Handle(GetListSystemUserQuery query, CancellationToken cancellationToken)
    {
        var querable = uofContext
            .SystemUsersRepository
            .AsQueryable(false);

        if (query.IncludePreferences == true)
            querable = querable.Include(i => i.Preferences);

        if (query.IncludeWatchlist == true)
            querable = querable.Include(i => i.Watchlist);

        if (query.Uuid is not null)
            querable = querable.Where(w => w.Uuid == query.Uuid);

        if (query.Username is not null)
            querable = querable.Where(w => w.Username == query.Username);

        if (query.Email is not null)
            querable = querable.Where(w => w.Email == query.Email);

        if (query.IsActive is not null)
            querable = querable.Where(w => w.IsActive == query.IsActive);

        if (query.IsVerified is not null)
            querable = querable.Where(w => w.IsVerified == query.IsVerified);

        // execute query on db
        return await uofContext
            .SystemUsersRepository
            .GetPaginatedListAsync(querable,
                                   query.PageNumber,
                                   query.PageSize,
                                   query.SortBy,
                                   cancellationToken);
    }
}