//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientGetUserSessionsEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("/client/openid/user/{email}/sessions", async ([FromRoute] string email,
//                                                                                   ISender sender,
//                                                                                   IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakClientGetUserSessionsQuery(email), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("GetUserSessionsInfo")
//        .WithSummary("Get all user sessions in keycloak")
//        .WithDescription("This endpoint allows you to retrieve user info created in keycloak.")
//        .WithTags("Client")
//        .Produces<KeycloakClientGetUserSessionsResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}