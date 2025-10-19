namespace FlixHub.Core.Api.Features.SystemUsers;

public sealed class LoginSystemUserEndpoint : IEndpointRoute
{
    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/auth/login", async ([FromBody] LoginSystemUserCommand command,
                                                           ISender sender,
                                                           IManagedCancellationToken applicationLifetime) =>
        {
            var response = await sender.Send(command, applicationLifetime.Token);

            return Results.Ok(response);
        })
        .WithName("LoginSystemUser")
        .WithSummary("Authorize system user")
        .WithDescription("This endpoint allows you to Authorize system user.")
        .WithTags("Authorization")
        .Produces<LoginSystemUserResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireApiKey()
        .WithOpenApi();
    }
}