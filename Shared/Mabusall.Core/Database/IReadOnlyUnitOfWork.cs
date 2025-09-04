namespace Mabusall.Core.Database;

public interface IReadOnlyUnitOfWork : IAsyncDisposable
{
    DbContext Context();
}