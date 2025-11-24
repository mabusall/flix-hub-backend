namespace FlixHub.Core.Database;

public interface IUnitOfWork : IAsyncDisposable
{
    DbContext Context();

    IExecutionStrategy CreateExecutionStrategy();

    IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Clears all tracked entities from the EF Core ChangeTracker to free memory.
    /// Should be called after SaveChangesAsync in batch operations.
    /// </summary>
    void ClearChangeTracker();
}