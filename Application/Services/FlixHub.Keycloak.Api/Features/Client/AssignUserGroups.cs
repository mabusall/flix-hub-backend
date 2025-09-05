//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientAssignUserGroupsEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("client/openid/user/{userId}/assign-groups", async ([FromRoute] Guid userId,
//                                                                                         [FromBody] KeycloakClientAssignUserGroupsCommand command,
//                                                                                         ISender sender,
//                                                                                         IManagedCancellationToken applicationLifetime) =>
//        {
//            command.Id = userId;
//            var response = await sender.Send(command, applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("AssignUserGroups")
//        .WithSummary("Assign user groups in keycloak for a specific realm")
//        .WithDescription("This endpoint allows you to Assign user groups in keycloak.")
//        .WithTags("Client")
//        .Produces<bool>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}