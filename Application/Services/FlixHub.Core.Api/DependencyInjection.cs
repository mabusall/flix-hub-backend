[assembly: InternalsVisibleTo("FlixHub.Api")]

namespace FlixHub.Core.Api;

static class DependencyInjection
{
    internal static IServiceCollection AddStoreModule(this IServiceCollection services,
                                                    IConfiguration configuration,
                                                    bool isDevelopment = false)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services
            .AddEndpoints(assembly)
            .AddValidatorsFromAssembly(assembly)

            // MediatR
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(assembly);
                config.AddOpenBehavior(typeof(FeatureBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            })

            .AddScoped<IFlixHubDbUnitOfWork, FlixHubDbUnitOfWork>()

            .AddDbContext<FlixHubDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("Default").Decrypt(),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null); // PostgreSQL uses error codes, not SQL Server numbers
                    })
                    .UseExceptionProcessor(); // make sure you installed EntityFrameworkCore.Exceptions.PostgreSQL

                if (isDevelopment)
                {
                    options.EnableSensitiveDataLogging();
                }
            });


        return services;
    }

    public static WebApplication RegisterStoreTasks(this WebApplication app)
    {
        //var hangfireOptions = app
        //    .Configuration
        //    .GetSection(HangfireOptions.ConfigurationKey)
        //    .Get<HangfireOptions>();

        //var jobManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<IRecurringJobManager>();
        //var storeScheduler = hangfireOptions!.Tasks[nameof(TasksScheduler.Store)];
        //var cron = storeScheduler.Schedule.ToCronExpression();

        //if (!storeScheduler.IsEnabled)
        //    jobManager.RemoveIfExists(storeScheduler.Id);
        //else
        //    TaskManager.RegisterHangfireJob<CachingStore>(jobManager,
        //                                                  storeScheduler.Id,
        //                                                  cron,
        //                                                  handler => handler.ExecuteAsync());

        return app;
    }
}
