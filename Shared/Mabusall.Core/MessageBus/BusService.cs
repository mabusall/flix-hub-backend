namespace Tasheer.Core.MessageBus;

public class BusService(IBus bus,
                        IPublishEndpoint outbox,
                        OutBoxDbContext dbContext,
                        ILogger<BusService> logger) : IBusService
{
    public async Task<TResponse> Request<TCommand, TResponse>(TCommand command)
        where TCommand : class
        where TResponse : class
    {
        var queueName = BusConfiguration.GetQueueName(command);
        var client = bus.CreateRequestClient<TCommand>(new Uri(queueName), TimeSpan.FromSeconds(10));
        var response = await client.GetResponse<TResponse>(command);

        return response.Message;
    }

    public async Task Publish<TEvent>(TEvent @event,
                                      CancellationToken cancellationToken)
        where TEvent : IBusEvent
    {
        if (@event.IsOutbox)
        {
            try
            {
                await outbox.Publish(@event, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while publishing outbox event {@event}", @event);
                throw new PublishOutboxException();
            }
        }
        else
        {
            await bus.Publish(@event, cancellationToken);
        }
    }
}