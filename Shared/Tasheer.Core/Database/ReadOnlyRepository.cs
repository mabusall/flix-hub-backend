namespace Tasheer.Core.Database;

public class ReadOnlyRepository<TEntity, TBrief>
    : IReadOnlyRepository<TEntity, TBrief>
    where TEntity : class
    where TBrief : class
{
    #region [ injected variables ]

    private readonly DbContext _context;
    private readonly DbSet<TEntity> _table;

    #endregion

    #region [ constructor ]

    public ReadOnlyRepository(DbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    #endregion

    #region [ database query ]

    /// <summary>
    /// Returns this object typed as IQueryable of TEntity
    /// </summary>
    /// <param name="trackable">
    /// If the value is false, The change tracker will not track any of the entities that are returned from a LINQ query.
    /// Otherwise, The change tracker will keep track of changes for all entities that are returned.
    /// 
    /// Keep notice that if trackable = true will degrade the performance sometimes
    /// </param>
    /// <returns></returns>
    public IQueryable<TEntity> AsQueryable() => _table.AsNoTracking();

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        => AsQueryable().AnyAsync(expression, cancellationToken);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        => AsQueryable().CountAsync(expression, cancellationToken);

    public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> query)
        => AsQueryable().Where(query).ToListAsync();

    public async Task<PaginatedList<TBrief>> GetPaginatedListAsync(IQueryable<TEntity> query,
                                                                   int? pageNumber,
                                                                   int? pageSize,
                                                                   CancellationToken cancellationToken)
    {
        var tableEntity = query ?? _table.AsNoTracking();
        int pageNumberCriteria = pageNumber is not null ? pageNumber.Value : 1;
        int pageSizeCriteria = pageSize is not null ? pageSize.Value : 10;

        var count = await tableEntity
            .TagWith("FORCE_LEGACY")
            .CountAsync(cancellationToken);

        var items = await tableEntity
            .Skip((pageNumberCriteria - 1) * pageSizeCriteria)
            .Take(pageSizeCriteria)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TBrief>(items.Adapt<List<TBrief>>(),
                                         count,
                                         pageNumberCriteria,
                                         pageSizeCriteria);
    }

    #endregion
}