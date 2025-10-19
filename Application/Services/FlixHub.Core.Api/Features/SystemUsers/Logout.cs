namespace FlixHub.Core.Api.Features.SystemUsers;

public sealed class LogoutSystemUserEndpoint : IEndpointRoute
{
    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/auth/logout", async ([FromBody] LogoutSystemUserCommand command,
                                                            ISender sender,
                                                            IManagedCancellationToken applicationLifetime) =>
        {
            var response = await sender.Send(command, applicationLifetime.Token);

            return Results.Ok(response);
        })
        .WithName("LogoutSystemUser")
        .WithSummary("Unauthorize system user")
        .WithDescription("This endpoint allows you to unauthorize system user.")
        .WithTags("Authorization")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireApiKey()
        .RequireAuthorization()
        .WithOpenApi();
    }
}