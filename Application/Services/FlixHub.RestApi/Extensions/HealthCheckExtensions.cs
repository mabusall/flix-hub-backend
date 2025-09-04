namespace Tasheer.Api.Extensions;

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
            .AddSqlServer(connectionString: configuration.GetConnectionString("Default").Decrypt(),
                          name: "sqlserver.Default",
                          tags: ["db", "sql", "sql.server", "Default"])
            .AddSqlServer(connectionString: rabbitMqConfig!.DbConnection.Decrypt(),
                          name: "sqlserver.OutBox",
                          tags: ["db", "sql", "sql.server", "OutBox"])
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