namespace Tasheer.Core.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public interface IVoidCommand : ICommand<Unit>
{
}
