namespace Mabusall.Notification.Helper;

public class EmailNotificationEvent : IBusEvent
{
    [Required] public string? Subject { get; set; }

    [Required] public string LanguageIsoCode { get; set; } = "en";

    [Required] public string? SiteUrl { get; set; }

    [Required] public dynamic? ExtraData { get; set; }

    [Required] public EmailNotificationAttachment? Attachment { get; set; }

    [Required] public EmailPriority Priority { get; set; }

    [Required] public EmailBodyTemplate Template { get; set; }

    [Required] public IEnumerable<string>? ToAddresses { get; set; }

    public IEnumerable<string>? CcAddresses { get; set; }

    public IEnumerable<string>? BccAddresses { get; set; }
}

public class EmailNotificationAttachment
{
    /// <summary>
    /// attachment model
    /// the data you want to use in the attachment
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// attachmant file name in email
    /// </summary>
    public string? FileName { get; set; }
}