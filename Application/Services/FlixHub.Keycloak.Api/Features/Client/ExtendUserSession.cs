//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientExtendUserSessionEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("/client/openid/sessions/extend", async ([FromBody] KeycloakClientExtendUserSessionCommand command,
//                                                                              ISender sender,
//                                                                              IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(command, applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("ExtendUserSession")
//        .WithSummary("Extend user session in keycloak")
//        .WithDescription("This endpoint allows you to kill user session in keycloak.")
//        .WithTags("Client")
//        .Produces<KeycloakClientExtendUserSessionResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}