//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientGetUserRolesEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("/client/openid/user/{userId}/roles", async ([FromRoute] Guid userId,
//                                                                                  ISender sender,
//                                                                                  IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakClientGetUserRolesQuery(userId), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("GetUserRoles")
//        .WithSummary("Get assigned user roles in keycloak")
//        .WithDescription("This endpoint allows you to retrieve assigned user roles in keycloak.")
//        .WithTags("Client")
//        .Produces<KeycloakClientGetUserRolesResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}