namespace FlixHub.Core.Api.Features.SystemUsers;

internal class LoginSystemUserCommandHandler(IFlixHubDbUnitOfWork uofContext,
                                             ISender sender,
                                             IMemoryCacheProvider cacheProvider,
                                             IBusService busService,
                                             IConfiguration configuration)
    : ICommandHandler<LoginSystemUserCommand, LoginSystemUserResult>
{
    public async Task<LoginSystemUserResult> Handle(LoginSystemUserCommand command, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new LoginSystemUserResult(Guid.NewGuid()));
    }
}