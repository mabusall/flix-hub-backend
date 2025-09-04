//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientDeleteUserEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("client/openid/users/{userId}/delete", async ([FromRoute] Guid userId,
//                                                                                   ISender sender,
//                                                                                   IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakClientDeleteUserCommand(userId), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("DeleteUser")
//        .WithSummary("Delete user in keycloak for a specific realm")
//        .WithDescription("This endpoint allows you to delete user in keycloak.")
//        .WithTags("Client")
//        .Produces<bool>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}