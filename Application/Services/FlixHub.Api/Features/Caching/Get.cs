namespace Store.Api.Features.Caching;

public sealed class GetListCacheEndpoint : IEndpointRoute
{
    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/store/cache", async ([AsParameters] GetListCacheQuery query,
                                                           ISender sender,
                                                           IManagedCancellationToken applicationLifetime) =>
        {
            var response = await sender.Send(query, applicationLifetime.Token);

            return Results.Ok(response);
        })
        .WithName("GetStoreCache")
        .WithSummary("Store caching")
        .WithDescription("This endpoint allows you retrieve cached object based on key.")
        .WithTags("Caching")
        .Produces<string>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireApiKey()
        .WithOpenApi();
    }
}