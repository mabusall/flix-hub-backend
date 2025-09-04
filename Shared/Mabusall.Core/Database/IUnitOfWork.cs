namespace Mabusall.Core.Database;

public interface IUnitOfWork : IAsyncDisposable
{
    DbContext Context();

    IExecutionStrategy CreateExecutionStrategy();

    IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}