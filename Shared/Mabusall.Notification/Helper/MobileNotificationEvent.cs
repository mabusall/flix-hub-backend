namespace Mabusall.Notification.Helper;

public class MobileNotificationEvent : IBusEvent
{
    [Required]
    public string? MessageTitleAr { get; set; }

    [Required]
    public string? MessageBodyAr { get; set; }

    [Required]
    public string? MessageTitleEn { get; set; }

    [Required]
    public string? MessageBodyEn { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public IList<string>? DeviceTokens { get; set; }
}
