//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientGetUserGroupsEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("/client/openid/user/{userId}/groups", async ([FromRoute] Guid userId,
//                                                                                  ISender sender,
//                                                                                  IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakClientGetUserGroupsQuery(userId), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("GetUserGroups")
//        .WithSummary("Get assigned user groups in keycloak")
//        .WithDescription("This endpoint allows you to retrieve assigned user groups in keycloak.")
//        .WithTags("Client")
//        .Produces<KeycloakClientGetUserGroupsResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}