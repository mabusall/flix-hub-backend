namespace FlixHub.Core.Api.Model;

public record NotificationSettingsDto
{
    public bool ShowDesktopNotifications { get; set; } = true;
    public bool EpisodeNotifications { get; set; } = true;
    public bool UpdateNotifications { get; set; } = true;
}
