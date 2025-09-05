namespace FlixHub.Api.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddApiHealthChecks(this IServiceCollection services,
                                                        IConfiguration configuration)
    {
        var rabbitMqConfig = configuration
            .GetSection(RabbitMqOptions.ConfigurationKey)
            .Get<RabbitMqOptions>();

        var redisConfig = configuration
            .GetSection(RedisOptions.ConfigurationKey)
            .Get<RedisOptions>();

        services.AddHealthChecks()
            .AddNpgSql(connectionString: configuration.GetConnectionString("Default").Decrypt(),
                       name: "postgres.Default",
                       tags: ["db", "sql", "postgres", "Default"])
            
            .AddNpgSql(connectionString: rabbitMqConfig!.DbConnection.Decrypt(),
                       name: "postgres.OutBox",
                       tags: ["db", "sql", "postgres", "OutBox"])
            
            .AddRedis(redisConnectionString: $"{redisConfig!.Uri},password={redisConfig.Password.Decrypt()},abortConnect=false",
                      name: "redis",
                      tags: ["rd", "redis"]);


        return services;
    }

    public static IApplicationBuilder UseApiHealthChecks(this IApplicationBuilder app)
    {
        return
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
    }
}