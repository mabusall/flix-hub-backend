//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientGetClientsEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("/client/openid/clients", async (ISender sender,
//                                                                    IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakClientGetClientsQuery(), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("ClientGetClients")
//        .WithSummary("Get all clients for the realm in keycloak")
//        .WithDescription("This endpoint allows you to retrieve all clients for the realm in keycloak.")
//        .WithTags("Client")
//        .Produces<KeycloakClientGetClientsResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}