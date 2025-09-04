namespace Mabusall.Notification.Helper;

public record FirebaseNotificationMessage
{
    public string? MessageTitleAr { get; set; }

    public string? MessageBodyAr { get; set; }

    public string? MessageTitleEn { get; set; }

    public string? MessageBodyEn { get; set; }

    public string? ImageUrl { get; set; }

    public List<string>? DeviceTokens { get; set; }
}
