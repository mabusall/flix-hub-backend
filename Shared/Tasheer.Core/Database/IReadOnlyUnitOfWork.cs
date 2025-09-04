namespace Tasheer.Core.Database;

public interface IReadOnlyUnitOfWork : IAsyncDisposable
{
    DbContext Context();
}