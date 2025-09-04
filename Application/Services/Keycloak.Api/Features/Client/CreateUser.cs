//namespace Keycloak.Api.Features.Client;

//public sealed class KeycloakClientCreateUserEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("client/openid/users", async ([FromBody] KeycloakClientCreateUserCommand command,
//                                                                   ISender sender,
//                                                                   IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(command, applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("CreatUser")
//        .WithSummary("Create user in keycloak for a specific realm")
//        .WithDescription("This endpoint allows you to create user in keycloak.")
//        .WithTags("Client")
//        .Produces<bool>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}