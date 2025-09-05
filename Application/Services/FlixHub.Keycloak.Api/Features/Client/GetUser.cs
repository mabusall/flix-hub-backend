//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientGetUserEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("/client/openid/user/{email}", async ([FromRoute] string email,
//                                                                          ISender sender,
//                                                                          IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakClientGetUserQuery(email), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("GetUserInfo")
//        .WithSummary("Get user info created in keycloak")
//        .WithDescription("This endpoint allows you to retrieve user info created in keycloak.")
//        .WithTags("Client")
//        .Produces<KeycloakClientGetUserResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}