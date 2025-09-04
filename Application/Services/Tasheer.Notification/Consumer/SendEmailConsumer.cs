namespace Tasheer.Notification.Consumer;

public class SendEmailConsumer(IAppSettingsKeyManagement appSettingsKeyManagement,
                               ILogger<SendEmailConsumer> logger,
                               IEmailTemplateRenderer emailTemplateRenderer)
        : IConsumer<EmailNotificationEvent>
{
    private readonly ILocalUtilityReportingService _localReporting
        = new LocalReporting()
            .UseBinary(JsReportBinary.GetBinary())
            .Configure(cfg => cfg.DoTrustUserCode().BaseUrlAsWorkingDirectory())
            .AsUtility()
            .Create();

    public async Task Consume(ConsumeContext<EmailNotificationEvent> context)
    {
        var @event = context.Message;
        var smtpEmailOptions = appSettingsKeyManagement.SmtpEmailOptions;
        var fromAddress = smtpEmailOptions!.SenderEmail;
        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromAddress, "Tasheer - تأشير"),
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

        if (@event.Attachment is not null)
        {
            try
            {
                var attachment = await RenderAttachmentBody(@event);
                mailMessage
                    .Attachments
                    .Add(new Attachment(await RenderPDF(attachment),
                    $"{@event.Attachment.FileName}.pdf", MediaTypeNames.Application.Pdf));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while sending email notification with attachment");
                throw;
            }
        }

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
            case NotificationTemplate.VerifyOTP:
                emailBody = await emailTemplateRenderer.
                       RenderRazorTemplateFromPathAsStringAsync($"~/Views/Account/VerifyOtp.cshtml",
                       new VerifyOtpModel
                       {
                           OTPValue = extraData.GetPropertyValue("otpValue"),
                           SiteUrl = extraData.GetPropertyValue("siteUrl"),
                           LanguageIsoCode = model.LanguageIsoCode
                       });
                break;
        }

        return emailBody;
    }

    private Task<string> RenderAttachmentBody(EmailNotificationEvent model)
    {
        return Task.FromResult("");
        //var attachmentBody = string.Empty;

        //switch (model.Attachment.Template)
        //{
        //    case AttachmentTemplate.Invoice:
        //        var invoiceViewModel = model.Attachment!.Body.Deserialize<InvoiceEmailViewModel>();

        //        attachmentBody = await _emailTemplateRenderer.
        //               RenderRazorTemplateFromPathAsStringAsync($"~/Views/Invoice/InvoiceAttachment.cshtml",
        //               new InvoiceAttachmentModel
        //               {
        //                   InvoiceViewModel = invoiceViewModel,
        //                   LanguageIsoCode = model.LanguageIsoCode,
        //                   SiteUrl = "https://hotelmedia.crs.haj.gov.sa/Haj.Nusuk",
        //                   DefaultClosingDate = invoiceViewModel.DefaultClosingDate,
        //                   AllowedHoursCount = invoiceViewModel.AllowedHoursCount,
        //                   ProductType = invoiceViewModel.ProductType,
        //               });
        //        break;

        //    case AttachmentTemplate.RefundInvoiceSummary:
        //        var refundInvoiceViewModel = model.Attachment.Body.Deserialize<RefundInvoiceEmailViewModel>();

        //        attachmentBody = await _emailTemplateRenderer.
        //               RenderRazorTemplateFromPathAsStringAsync($"~/Views/Invoice/RefundInvoiceAttachment.cshtml",
        //               new RefundInvoiceAttachmentModel
        //               {
        //                   RefundInvoiceViewModel = refundInvoiceViewModel,
        //                   LanguageIsoCode = model.LanguageIsoCode,
        //                   SiteUrl = "https://hotelmedia.crs.haj.gov.sa/Haj.Nusuk"
        //               });
        //        break;

        //        //case AttachmentTemplate.TourGuideContract:
        //        //    var tourGuideContractViewModel = model.Attachment.Body.Deserialize<TourGuideContractView>();

        //        //    attachmentBody = await _emailTemplateRenderer.
        //        //           RenderRazorTemplateFromPathAsStringAsync($"~/Views/TourGuides/TGContractTemplate.cshtml",
        //        //           new TourGuideContractTemplateModel
        //        //           {
        //        //               Body = tourGuideContractViewModel,
        //        //               LanguageIsoCode = model.LanguageIsoCode,
        //        //               SiteUrl = "https://hotelmedia.crs.haj.gov.sa/Haj.Nusuk"
        //        //           });
        //        // break;
        //}

        //return attachmentBody;
    }

    private async Task<Stream> RenderPDF(string coverHtml)
    {
        // https://jsreport.net/learn/dotnet-local
        // https://github.com/jsreport/jsreport-dotnet-example-docker

        var report = await _localReporting.RenderAsync(new RenderRequest()
        {
            Template = new Template()
            {
                Recipe = Recipe.ChromePdf,
                Engine = Engine.None,
                Content = coverHtml
            }
        });

        return report.Content;
    }

    #endregion
}