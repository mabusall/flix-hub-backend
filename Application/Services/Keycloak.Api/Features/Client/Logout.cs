//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientLogoutEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("client/openid/logout", async ([FromBody] KeycloakClientLogoutCommand command,
//                                                                    ISender sender,
//                                                                    IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(command, applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("ClientLogout")
//        .WithSummary("Logout using keycloak")
//        .WithDescription("This endpoint allows you to logout the user using refresh token generated after login.")
//        .WithTags("Client")
//        .Produces<KeycloakClientLogoutResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireAuthorization()
//        .WithOpenApi();
//    }
//}