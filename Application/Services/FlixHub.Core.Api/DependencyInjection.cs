[assembly: InternalsVisibleTo("FlixHub.Api")]

namespace FlixHub.Core.Api;

static class DependencyInjection
{
    internal static IServiceCollection AddFlixHubModule(this IServiceCollection services,
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
                //config.AddOpenBehavior(typeof(FeatureBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            })

            // Register TMDB/OMDb/Trakt services
            .AddScoped<TmdbService>()
            .AddScoped<OmdbService>()
            //.AddScoped<TraktService>()
            .AddSingleton<ITokenRing>(sp =>
            {
                var cache = sp.GetRequiredService<IMemoryCacheProvider>();
                var appSettings = sp.GetRequiredService<IAppSettingsKeyManagement>();
                var omdb = appSettings.IntegrationApisOptions.Apis["OMDB"];
                var tokens = omdb.Tokens.Select(t => t.Decrypt()).ToList(); // your Decrypt()

                // cacheKey must be unique per API
                return new CachedTokenRing(cache, "tokenring:omdb", tokens);
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

    public static WebApplication RegisterTypedTasks(this WebApplication app)
    {
        var hangfireOptions = app
            .Configuration
            .GetSection(HangfireOptions.ConfigurationKey)
            .Get<HangfireOptions>()!;

        var jobManager = app.Services.GetRequiredService<IRecurringJobManager>();

        TaskRegister.Register(app, jobManager, hangfireOptions);

        return app;
    }
}
