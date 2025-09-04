namespace Mabusall.Notification.Consumer;

public class SendEmailConsumer(IAppSettingsKeyManagement appSettingsKeyManagement,
                               IEmailTemplateRenderer emailTemplateRenderer)
        : IConsumer<EmailNotificationEvent>
{
    public async Task Consume(ConsumeContext<EmailNotificationEvent> context)
    {
        var @event = context.Message;
        var smtpEmailOptions = appSettingsKeyManagement.SmtpEmailOptions;
        var fromAddress = smtpEmailOptions!.SenderEmail;
        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromAddress, "XTeam - فريق العمل"),
            Subject = @event.Subject,
            IsBodyHtml = true,
            Body = await RenderEmailBody(@event),
        };

        // TO Address
        if (@event.ToAddresses is not null)
        {
            foreach (var address in @event.ToAddresses!.Select(s => new MailAddress(s)))
                mailMessage.To.Add(address);
        }

        // CC Address
        if (@event.CcAddresses is not null)
        {
            foreach (var address in @event.CcAddresses!.Select(s => new MailAddress(s)))
                mailMessage.CC.Add(address);
        }

        // BCC Address
        if (@event.BccAddresses is not null)
        {
            foreach (var address in @event.BccAddresses!.Select(s => new MailAddress(s)))
                mailMessage.Bcc.Add(address);
        }

        using var smtpClient = new SmtpClient
        {
            Host = smtpEmailOptions!.Host,
            EnableSsl = smtpEmailOptions!.EnableSsl,
            Port = smtpEmailOptions!.Port,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress, smtpEmailOptions!.Password.Decrypt()),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Timeout = smtpEmailOptions!.Timeout
        };

        // everything is ok... so far
        await smtpClient.SendMailAsync(mailMessage);
    }

    #region [ rendering ]

    private async Task<string> RenderEmailBody(EmailNotificationEvent model)
    {
        var emailBody = string.Empty;
        var extraData = (JsonElement)model.ExtraData;

        switch (model.Template)
        {
            case EmailBodyTemplate.VerifyOTP:
                emailBody = await emailTemplateRenderer.
                       RenderRazorTemplateFromPathAsStringAsync($"~/Views/Account/VerifyOtp.cshtml",
                       new VerifyOtpModel
                       {
                           OtpValue = extraData.GetPropertyValue("otpValue"),
                           SiteUrl = model.SiteUrl,
                           LanguageIsoCode = model.LanguageIsoCode
                       });
                break;

            case EmailBodyTemplate.VerifyEmail:
                emailBody = await emailTemplateRenderer.
                       RenderRazorTemplateFromPathAsStringAsync($"~/Views/Account/VerifyEmail.cshtml",
                       new VerifyEmailModel
                       {
                           RedirectUrl = extraData.GetPropertyValue("redirectUrl"),
                           ServiceProviderName = extraData.GetPropertyValue("name"),
                           UserTypeId = int.Parse(extraData.GetPropertyValue("userTypeId")!),
                           SiteUrl = model.SiteUrl,
                           LanguageIsoCode = model.LanguageIsoCode
                       });
                break;

            case EmailBodyTemplate.ForgetPassword:
                emailBody = await emailTemplateRenderer.
                       RenderRazorTemplateFromPathAsStringAsync($"~/Views/Account/ForgetPassword.cshtml",
                       new ForgetPasswordModel
                       {
                           RedirectUrl = extraData.GetPropertyValue("redirectUrl"),
                           SiteUrl = model.SiteUrl,
                           LanguageIsoCode = model.LanguageIsoCode
                       });
                break;
        }

        return emailBody;
    }

    #endregion
}