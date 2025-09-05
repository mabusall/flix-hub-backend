//namespace Keycloak.Api.Features.Admin;

//public sealed class KeycloakAdminClientsEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapGet("/admin/openid/clients", async (ISender sender,
//                                                                    IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(new KeycloakAdminClientsQuery(), applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("AdminGetClients")
//        .WithSummary("Get all clients for the admin user using keycloak")
//        .WithDescription("This endpoint allows you to retrieve all clients for the logged in admin user.")
//        .WithTags("Admin")
//        .Produces<KeycloakAdminClientsResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        .WithOpenApi();
//    }
//}