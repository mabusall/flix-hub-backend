//namespace Keycloak.Api.Features.Admin;

//public sealed class KeycloakAdminLoginEndpoint : IEndpointRoute
//{
//    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        endpointRouteBuilder.MapPost("/admin/openid/login", async ([FromBody] KeycloakAdminLoginCommand command,
//                                                                   ISender sender,
//                                                                   IManagedCancellationToken applicationLifetime) =>
//        {
//            var response = await sender.Send(command, applicationLifetime.Token);

//            return Results.Ok(response);
//        })
//        .WithName("AdminLogin")
//        .WithSummary("Admin login using keycloak")
//        .WithDescription("This endpoint allows you to generate token using keycloak provider.")
//        .WithTags("Admin")
//        .Produces<KeycloakAdminLoginResult>(StatusCodes.Status200OK)
//        .ProducesProblem(StatusCodes.Status400BadRequest)
//        .ProducesProblem(StatusCodes.Status404NotFound)
//        .RequireApiKey()
//        //.RequirePermissions(EmailPriority.Low, EmailPriority.High)
//        //.RequirePermissions(1, 2)
//        .WithOpenApi();
//    }
//}