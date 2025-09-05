//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientRevokeUserGroupsEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("client/openid/user/{userId}/revoke-groups", async ([FromRoute] Guid userId,
//                                                                                         [FromBody] KeycloakClientRevokeUserGroupsCommand command,
//                                                                                         ISender sender,
//                                                                                         IManagedCancellationToken applicationLifetime) =>
//        {
//            command.Id = userId;
//            var response = await sender.Send(command, applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("RevokeUserGroups")
//        .WithSummary("Revoke user groups in keycloak for a specific realm")
//        .WithDescription("This endpoint allows you to Revoke user groups in keycloak.")
//        .WithTags("Client")
//        .Produces<bool>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}