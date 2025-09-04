namespace Tasheer.Core.Database;

public interface IReadOnlyRepository<TEntity, TBrief>
    where TEntity : class
    where TBrief : class
{
    #region [ database query ]

    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression);

    Task<PaginatedList<TBrief>> GetPaginatedListAsync(IQueryable<TEntity> query,
                                                      int? pageNumber,
                                                      int? pageSize,
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
    IQueryable<TEntity> AsQueryable();

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression,
                        CancellationToken cancellationToken);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression,
                         CancellationToken cancellationToken);

    #endregion
}