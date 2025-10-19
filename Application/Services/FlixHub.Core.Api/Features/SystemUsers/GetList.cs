namespace FlixHub.Core.Api.Features.SystemUsers;

public sealed class GetListSystemUserEndpoint : IEndpointRoute
{
    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/um/users", async ([AsParameters] GetListSystemUserQuery query,
                                                        ISender sender,
                                                        IManagedCancellationToken applicationLifetime) =>
        {
            var response = await sender.Send(query, applicationLifetime.Token);

            return Results.Ok(response);
        })
        .WithName("GetListSystemUser")
        .WithSummary("Filter system users")
        .WithDescription("This endpoint allows you to filter system users.")
        .WithTags("User Managements")
        .Produces<PaginatedList<SystemUserDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireApiKey()
        .RequireAuthorization()
        .WithOpenApi();
    }
}