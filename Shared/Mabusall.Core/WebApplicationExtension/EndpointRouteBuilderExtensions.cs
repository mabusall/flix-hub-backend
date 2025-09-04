namespace Mabusall.Core.WebApplicationExtension;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder RegisterEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var registeredEndpoints = endpointRouteBuilder
            .ServiceProvider
            .GetServices<IEndpointRoute>();

        foreach (var endpointRoute in registeredEndpoints)
            endpointRoute.AddRoute(endpointRouteBuilder);

        return endpointRouteBuilder;
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly currentAssembly)
    {
        // get slices 
        var endpointRoutes = currentAssembly
            .GetTypes()
            .Where(w => typeof(IEndpointRoute).IsAssignableFrom(w) &&
                        w != typeof(IEndpointRoute) &&
                        w.IsPublic &&
                        !w.IsAbstract);

        // register them as singletons
        foreach (var endpointRoute in endpointRoutes)
            services.AddSingleton(typeof(IEndpointRoute), endpointRoute);

        return services;
    }

    public static TBuilder RequireApiKey<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddEndpointFilter(new ApiKeyFilter());
    }

    public static TBuilder RequirePermissions<TBuilder, TPermission>
        (this TBuilder builder, params TPermission[] requiredPermissions)
        where TBuilder : IEndpointConventionBuilder
        where TPermission : struct
    {
        // Safely convert to int
        var permissionValues = requiredPermissions
            .Select(permission => Convert.ToInt32(permission))
            .ToArray();

        return builder.AddEndpointFilter(new PermissionFilter(permissionValues));
    }

    public static TBuilder RequireValidToken<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddEndpointFilter(new ValidTokenFilter());
    }
}