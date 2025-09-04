namespace Mabusall.Core.Database;

public class UnitOfWork(DbContext context) : IUnitOfWork
{
    /// <summary>
    /// Creates an instance of the configured ExecutionStrategy.
    /// </summary>
    public IExecutionStrategy CreateExecutionStrategy()
        => context.Database.CreateExecutionStrategy();

    /// <summary>
    /// Starts a new transaction with a given System.Data.IsolationLevel.
    /// </summary>
    public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        => context.Database.BeginTransaction(isolationLevel);

    /// <summary>
    /// don't use it, ever never. !!!!!
    /// </summary>
    public DbContext Context() => context;

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => context.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Releases the allocated resources for this context.
    /// </summary>
    public async ValueTask DisposeAsync()
        => await context.DisposeAsync();
}