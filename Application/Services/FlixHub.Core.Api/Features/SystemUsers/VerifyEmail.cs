namespace FlixHub.Core.Api.Features.SystemUsers;

public sealed class VerifyEmailSystemUserEndpoint : IEndpointRoute
{
    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/um/users/verify-email", async ([FromBody] VerifyEmailSystemUserCommand command,
                                                                      ISender sender,
                                                                      IManagedCancellationToken applicationLifetime) =>
        {
            var response = await sender.Send(command, applicationLifetime.Token);

            return Results.Ok(response);
        })
        .WithName("VerifyEmailSystemUser")
        .WithSummary("Verify email for new registered system user")
        .WithDescription("This endpoint allows you to verify email for new registered system user.")
        .WithTags("User Managements")
        .Produces<VerifyEmailSystemUserResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireApiKey()
        .WithOpenApi();
    }
}