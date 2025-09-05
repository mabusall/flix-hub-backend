namespace FlixHub.Core.Database;

public class GenericRepository<TEntity, TBrief>
    : IGenericRepository<TEntity, TBrief>
    where TEntity : AuditableEntity
    where TBrief : class
{
    #region [ injected variables ]

    private readonly DbSet<TEntity> _table;

    public DbSet<TEntity> Table => _table;

    #endregion

    #region [ constructor ]

    public GenericRepository(DbContext context)
    {
        _table = context.Set<TEntity>();
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
    public IQueryable<TEntity> AsQueryable(bool trackable) =>
        trackable ?
        _table.AsTracking()
        :
        _table.AsNoTracking();

    public Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        => Task.FromResult(_table.Find(id));

    public Task<TEntity> GetByUuidAsync(Guid uuid, CancellationToken cancellationToken)
        => AsQueryable(true).SingleOrDefaultAsync(e => e.Uuid == uuid, cancellationToken);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        => AsQueryable(false).AnyAsync(expression, cancellationToken);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        => AsQueryable(false).CountAsync(expression, cancellationToken);

    public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> query, CancellationToken cancellationToken)
        => AsQueryable(false).Where(query).ToListAsync(cancellationToken);

    public async Task<PaginatedList<TBrief>> GetPaginatedListAsync(IQueryable<TEntity> query,
                                                                   int? pageNumber,
                                                                   int? pageSize,
                                                                   string sortBy,
                                                                   CancellationToken cancellationToken)
    {
        var tableEntity = query ?? _table.AsNoTracking();
        int pageNumberCriteria = pageNumber is not null ? pageNumber.Value : 1;
        int pageSizeCriteria = pageSize is not null ? pageSize.Value : 10;

        if (string.IsNullOrWhiteSpace(sortBy))
            sortBy = "Id ASC";

        if (!string.IsNullOrWhiteSpace(sortBy))
            tableEntity = tableEntity.OrderBy(sortBy);

        var count = await tableEntity
            .TagWith("FORCE_LEGACY")
            .CountAsync(cancellationToken);

        var items = await tableEntity
            .Skip((pageNumberCriteria - 1) * pageSizeCriteria)
            .Take(pageSizeCriteria)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return new PaginatedList<TBrief>(items.Adapt<List<TBrief>>(),
                                         count,
                                         pageNumberCriteria,
                                         pageSizeCriteria);
    }

    #endregion

    #region [ database context actions ]

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    public TEntity Insert(TEntity entity)
        => _table.Add(entity).Entity;

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    public void InsertRange(TEntity[] entities)
        => _table.AddRange(entities);

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    public void Update(TEntity entity)
        => _table.Update(entity);

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    public void UpdateRange(TEntity[] entities)
        => _table.UpdateRange(entities);

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    public void DeleteByQuery(Expression<Func<TEntity, bool>> predicate)
    {
        var items = AsQueryable(true)
            .Where(predicate)
            .ToList();

        if (items is not null && items.Count != 0)
            _table.RemoveRange(items);
    }

    public void Delete(TEntity entity)
    {
        if (entity is null)
            throw new NullReferenceException(nameof(TEntity));

        _table.Remove(entity);
    }

    #endregion
}