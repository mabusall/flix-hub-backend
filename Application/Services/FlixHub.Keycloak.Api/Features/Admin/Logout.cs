//namespace Keycloak.Api.Features.Admin;

//public sealed class KeycloakAdminLogoutEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("admin/openid/logout", async ([FromBody] KeycloakAdminLogoutCommand command,
//                                                                    ISender sender,
//                                                                    IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(command, applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("AdminLogout")
//        .WithSummary("Logout using keycloak")
//        .WithDescription("This endpoint allows you to logout the user using refresh token generated after login.")
//        .WithTags("Admin")
//        .Produces<KeycloakAdminLogoutResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}