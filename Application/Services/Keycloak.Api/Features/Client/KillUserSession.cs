//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientKillUserSessionEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("/client/openid/sessions/{sessionId}/kill", async ([FromRoute] Guid sessionId,
//                                                                                            ISender sender,
//                                                                                            IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakClientKillUserSessionCommand(sessionId), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("KillUserSession")
//        .WithSummary("Kill user session in keycloak")
//        .WithDescription("This endpoint allows you to kill user session in keycloak.")
//        .WithTags("Client")
//        .Produces<KeycloakClientKillUserSessionResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}