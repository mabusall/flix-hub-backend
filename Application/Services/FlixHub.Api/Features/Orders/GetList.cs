namespace Store.Api.Features.Orders;

public sealed class GetListOrderEndpoint : IEndpointRoute
{
    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/store/orders", async ([AsParameters] GetListOrderQuery query,
                                                            ISender sender,
                                                            IManagedCancellationToken applicationLifetime) =>
        {
            var response = await sender.Send(query, applicationLifetime.Token);

            return Results.Ok(response);
        })
        .WithName("GetListOrder")
        .WithSummary("Filter orders")
        .WithDescription("This endpoint allows you to filter orders by parameters.")
        .WithTags("Store")
        .Produces<PaginatedList<Order>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        //.CheckTokenValidity()
        .WithOpenApi();
    }
}