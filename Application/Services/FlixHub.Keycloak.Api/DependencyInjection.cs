[assembly: InternalsVisibleTo("FlixHub.Api")]

namespace FlixHub.Keycloak.Api;

static class DependencyInjection
{
    internal static IServiceCollection AddKeycloakModule(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services
            .AddEndpoints(assembly)
            .AddValidatorsFromAssembly(assembly)

            // MediatR
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));

            });

        return services;
    }
}
