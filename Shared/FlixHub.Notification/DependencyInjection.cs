namespace FlixHub.Notification;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services,
                                                           IConfiguration configuration)
    {
        var rabbitMqOptions = configuration
            .GetSection(RabbitMqOptions.ConfigurationKey)
            .Get<RabbitMqOptions>();

        // Firebase configuration - updated approach
        var firebaseOptions = configuration
            .GetSection(FirebaseOptions.ConfigurationKey)
            .Get<FirebaseOptions>();

        foreach (var endpoint in rabbitMqOptions!.Consumers.Values)
        {
            if (endpoint!.IsActive)
            {
                // Use the current executing assembly to resolve the type
                var consumerType = Assembly
                    .GetExecutingAssembly()
                    .GetType(endpoint.Consumer);

                BusConfiguration.AddBusConfigurator(busConfigurator =>
                {
                    busConfigurator.AddConsumer(consumerType);
                });

                BusConfiguration.AddBusFactoryConfigurator((busFactoryConfigurator, registration) =>
                {
                    busFactoryConfigurator.ReceiveEndpoint(endpoint.Name,
                        e =>
                        {
                            e.ConfigureConsumer(registration, consumerType);
                            e.UseMessageRetry(r => r.Interval(endpoint.RetryCount, endpoint.Interval));
                            e.PrefetchCount = endpoint.PrefetchCount;
                            e.ConcurrentMessageLimit = endpoint.ConcurrentMessageLimit;

                            if (endpoint.UseDatabase)
                            {
                                e.UseEntityFrameworkOutbox<OutBoxDbContext>(registration);
                            }
                        });
                });
            }
        }

        if (firebaseOptions?.IsActive == true)
        {
            // Initialize Firebase app if it's not already initialized
            if (FirebaseApp.DefaultInstance is null)
            {
                var jsonCredential = JsonSerializerHandler.Serialize(firebaseOptions.FirebaseConfiguration);
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(jsonCredential)
                });
            }
        }

        services
            .AddScoped<IEmailTemplateRenderer, EmailTemplateRenderer>()
            .AddSingleton<IMobileNotificationService, MobileNotificationService>()
            .AddRazorPages();

        return services;
    }
}