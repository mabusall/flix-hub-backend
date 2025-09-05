namespace FlixHub.Core.Database;

public interface IGenericRepository<TEntity, TBrief>
    where TEntity : AuditableEntity
    where TBrief : class
{
    DbSet<TEntity> Table { get; }

    #region [ database query ]

    Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<TEntity> GetByUuidAsync(Guid uuid, CancellationToken cancellationToken);

    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);

    Task<PaginatedList<TBrief>> GetPaginatedListAsync(IQueryable<TEntity> query,
                                                      int? pageNumber,
                                                      int? pageSize,
                                                      string sortBy,
                                                      CancellationToken cancellationToken);

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
    IQueryable<TEntity> AsQueryable(bool trackable);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression,
                        CancellationToken cancellationToken);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression,
                         CancellationToken cancellationToken);

    #endregion

    #region [ database context actions ]

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    TEntity Insert(TEntity entity);

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    void InsertRange(TEntity[] entities);

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    void Update(TEntity entity);

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    void UpdateRange(TEntity[] entities);

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    void Delete(TEntity entity);

    /// <summary>
    /// SaveChanges() or SaveChangesAsync() must be called to apply all database changes
    /// </summary>
    void DeleteByQuery(Expression<Func<TEntity, bool>> predicate);

    #endregion
}