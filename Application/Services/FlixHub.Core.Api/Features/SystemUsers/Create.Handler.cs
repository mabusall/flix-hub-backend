namespace FlixHub.Core.Api.Features.SystemUsers;

internal class CreateSystemUserCommandHandler(IFlixHubDbUnitOfWork uow,
                                              IBusService busService,
                                              IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<CreateSystemUserCommand, CreateSystemUserResult>
{
    public async Task<CreateSystemUserResult> Handle(CreateSystemUserCommand command, CancellationToken cancellationToken)
    {
        var user = command.Adapt<SystemUser>();
        var httpContext = httpContextAccessor.HttpContext!;
        var request = httpContext.Request;
        // Build the root URL
        var siteUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

        // generate email verification code
        user.EmailVerificationCode = $"{Guid.NewGuid():N}".Encrypt();

        // add user to database
        uow.SystemUsersRepository.Insert(user);

        // commit the changes
        await uow.SaveChangesAsync(cancellationToken);

        // send email verification request
        await busService.Publish(new EmailNotificationEvent
        {
            Subject = EmailNotificationResources.ActivateAccountTemplate_Title,
            LanguageIsoCode = LocalizationInterpretor.CurrentLanguage(),
            Priority = EmailPriority.High,
            Template = EmailBodyTemplate.ActivateAccount,
            ToAddresses = [command.Email!],
            SiteUrl = siteUrl,
            ExtraData = new
            {
                Name = string.Join(' ', user.FirstName, user.LastName),
                Account = user.Username,
                user.Email,
                ActivationCode = user.EmailVerificationCode,
            },
        }, cancellationToken);

        return new CreateSystemUserResult(true);
    }
}