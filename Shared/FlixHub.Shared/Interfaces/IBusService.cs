namespace FlixHub.Shared.Interfaces;

public abstract class IBusEvent
{
    /// <summary>
    /// Set IsOutbox = true incase you want to publish the message using outbox pattern
    /// </summary>
    public bool IsOutbox { get; set; } = true;

    public DateTime Created { get; set; } = DateTime.UtcNow;
}

public interface IBusService
{
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : IBusEvent;

    Task<TResponse> Request<TCommand, TResponse>(TCommand command)
        where TCommand : class
        where TResponse : class;
}
