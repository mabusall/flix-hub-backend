namespace Tasheer.Notification;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services,
                                                           IConfiguration configuration)
    {
        var rabbitMqOptions = configuration
            .GetSection(RabbitMqOptions.ConfigurationKey)
            .Get<RabbitMqOptions>();

        var emailEndpoint = rabbitMqOptions!.Consumers["EmailEndpoint"];
        if (rabbitMqOptions!.IsActive && emailEndpoint!.IsActive)
        {
            // Use the current executing assembly to resolve the type
            var type = Assembly.GetExecutingAssembly().GetType(emailEndpoint.Consumer);

            BusConfiguration.AddBusConfigurator(busConfigurator =>
            {
                busConfigurator.AddConsumer(type);
            });

            BusConfiguration.AddBusFactoryConfigurator((busFactoryConfigurator, registration) =>
            {
                busFactoryConfigurator.ReceiveEndpoint(emailEndpoint.Name,
                    e =>
                    {
                        e.ConfigureConsumer(registration, type);

                        // retry 10 times with 5 seconds interval
                        e.UseMessageRetry(r => r.Interval(emailEndpoint.RetryCount, emailEndpoint.Interval));

                        // process one message only
                        e.PrefetchCount = emailEndpoint.PrefetchCount;
                        e.ConcurrentMessageLimit = emailEndpoint.ConcurrentMessageLimit;

                        // ensuring message durability and exactly - once message processing.
                        // It is used in the context of integrating RabbitMQ with Outbox Pattern
                        // and Entity Framework to handle message publishing in a reliable way,
                        // especially when you want to ensure that a message is only published once
                        // and to guarantee that no messages are lost during the process.
                        //if (emailEndpoint.UseDatabase)
                        //{
                        //e.UseEntityFrameworkOutbox<OutBoxDbContext>(registration);
                        //}
                    });
            });
        }
        services
            .AddScoped<IEmailTemplateRenderer, EmailTemplateRenderer>()
            .AddRazorPages();

        return services;
    }
}
