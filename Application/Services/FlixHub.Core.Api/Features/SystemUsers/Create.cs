namespace FlixHub.Core.Api.Features.SystemUsers;

public sealed class CreateSystemUserEndpoint : IEndpointRoute
{
    public void AddRoute(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/um/users", async ([FromBody] CreateSystemUserCommand command,
                                                          ISender sender,
                                                          IManagedCancellationToken applicationLifetime) =>
        {
            command.Password = command.Password.Decrypt();
            var response = await sender.Send(command, applicationLifetime.Token);

            return Results.Ok(response);
        })
        .WithName("CreateSystemUser")
        .WithSummary("Create a new system user")
        .WithDescription("This endpoint allows you to create a new system user.")
        .WithTags("User Managements")
        .Produces<CreateSystemUserResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireApiKey()
        .WithOpenApi();
    }
}