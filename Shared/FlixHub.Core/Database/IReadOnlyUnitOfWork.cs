namespace FlixHub.Core.Database;

public interface IReadOnlyUnitOfWork : IAsyncDisposable
{
    DbContext Context();
}