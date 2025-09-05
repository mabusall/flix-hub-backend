//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientGetGroupsEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("/client/openid/groups", async (ISender sender,
//                                                                    IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakClientGetGroupsQuery(), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("GetGroups")
//        .WithSummary("Get all clients for the admin user using keycloak")
//        .WithDescription("This endpoint allows you to retrieve all clients for the logged in admin user.")
//        .WithTags("Client")
//        .Produces<KeycloakClientGetGroupsResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}