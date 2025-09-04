//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientGetUserIntrospectEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("/client/openid/introspect", async (ISender sender,
//                                                                        HttpContext httpContext,
//                                                                        IManagedCancellationToken applicationLifetime) =>
//        {
//            var accessToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
//            var response = await sender.Send(new KeycloakClientGetUserIntrospectQuery(accessToken!), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("GetUserIntrospectInfo")
//        .WithSummary("Get introspect info in keycloak")
//        .WithDescription("This endpoint allows you to retrieve user info created in keycloak.")
//        .WithTags("Client")
//        .Produces<KeycloakClientGetUserIntrospectResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireAuthorization()
//        .WithOpenApi();
//    }
//}