namespace Tasheer.Shared.Helper;

public static class BusConfiguration
{
    private static Action<IBusRegistrationConfigurator> _busConfigurators;
    private static Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext> _busFactoryConfigurators;
    private static readonly Dictionary<string, string> _queues = [];

    public static void AddBusConfigurator(Action<IBusRegistrationConfigurator> busConfigurator)
    {
        _busConfigurators += busConfigurator;
    }

    public static Action<IBusRegistrationConfigurator> GetBusConfigurators()
        => _busConfigurators;

    public static void AddBusFactoryConfigurator(Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext> busFactoryConfigurator)
        => _busFactoryConfigurators += busFactoryConfigurator;

    public static Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext> GetBusFactoryConfigurators()
        => _busFactoryConfigurators;

    public static void AddQueueName<TEvent>(string queue)
        => _queues.Add(typeof(TEvent).Name, queue);

    public static string GetQueueName<TCommand>(TCommand command) where TCommand : class
        => $"queue:{_queues[command.GetType().Name]}";
}