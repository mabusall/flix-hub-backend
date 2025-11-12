namespace FlixHub.Core.Api.Features.SystemUsers;

internal class LoginSystemUserCommandHandler(IFlixHubDbUnitOfWork uow,
                                             IMemoryCacheProvider cacheProvider,
                                             IAppSettingsKeyManagement appSettings)
    : ICommandHandler<LoginSystemUserCommand, LoginSystemUserResult>
{
    public async Task<LoginSystemUserResult> Handle(LoginSystemUserCommand command, CancellationToken cancellationToken)
    {
        var password = command.Password.Decrypt();
        var user = await uow
                .SystemUsersRepository
                .AsQueryable(false)
                .FirstOrDefaultAsync(user => (user.Email == command.EmailOrAccount || user.Username == command.EmailOrAccount) &&
                                     user.Password == password, cancellationToken);
        var fullName = string.Join(' ', user!.FirstName!.Trim(), user.LastName!.Trim()).Trim();
        var cachedToken = await cacheProvider.GetAsync<string>(user!.Email, cancellationToken);

        if (!string.IsNullOrWhiteSpace(cachedToken))
            return new LoginSystemUserResult(fullName, cachedToken);

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(appSettings.JwtSecurityToken.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Uuid.ToString("N")),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.GivenName, string.Join(' ', user.FirstName,user.LastName))
            ]),
            Expires = null, // Token never expires
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = appSettings.JwtSecurityToken.Issuer,
            Audience = appSettings.JwtSecurityToken.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var newToken = tokenHandler.WriteToken(token);

        // Cache the token
        await cacheProvider.SetAsync(user.Email, newToken, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = null
        }, cancellationToken);

        return new LoginSystemUserResult(fullName, newToken);
    }
}