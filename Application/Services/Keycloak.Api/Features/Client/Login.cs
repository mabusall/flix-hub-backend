//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientLoginEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("/client/openid/login", async ([FromBody] KeycloakClientLoginCommand command,
//                                                                    ISender sender,
//                                                                    IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(command, applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("ClientLogin")
//        .WithSummary("Client login using keycloak")
//        .WithDescription("This endpoint allows you to generate token using keycloak provider.")
//        .WithTags("Client")
//        .Produces<KeycloakClientLoginResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}