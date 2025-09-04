namespace Tasheer.Shared.Helper;

public sealed class ManagedCancellationToken(CancellationToken cancellationToken) : IManagedCancellationToken
{
    public CancellationToken Token => cancellationToken;
}

public interface IManagedCancellationToken
{
    CancellationToken Token { get; }
}