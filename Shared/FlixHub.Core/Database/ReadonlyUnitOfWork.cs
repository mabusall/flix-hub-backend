namespace FlixHub.Core.Database;

public class ReadOnlyUnitOfWork(DbContext context) : IReadOnlyUnitOfWork
{
    /// <summary>
    /// don't use it, ever never. !!!!!
    /// </summary>
    public DbContext Context() => context;

    /// <summary>
    /// Releases the allocated resources for this context.
    /// </summary>
    public async ValueTask DisposeAsync()
        => await context.DisposeAsync();
}