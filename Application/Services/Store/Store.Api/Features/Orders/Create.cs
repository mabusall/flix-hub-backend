namespace Store.Api.Features.Orders;

public sealed class CreateOrderEndpoint : IEndpointRoute
{
    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/store/order", async ([FromBody] CreateOrderCommand command,
                                                            ISender sender,
                                                            IManagedCancellationToken applicationLifetime) =>
        {
            var response = await sender.Send(command,
                applicationLifetime.Token);
            var result = response.Adapt<CreateOrderResult>();

            return Results.Ok(result);
        })
        .WithName("CreateOrder")
        .WithSummary("Create a new order")
        .WithDescription("This endpoint allows you to create a new order by providing the required details, including price, VAT, items, and order type.")
        .WithTags("Store")
        .Produces<CreateOrderResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithOpenApi();
    }
}