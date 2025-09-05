namespace FlixHub.Core.Api.Model;

public record UserPreferencesDto
{
    public string Theme { get; set; } = "dark";
    public string AccentColor { get; set; } = "purple";
    public string Language { get; set; } = "en";

    // Appearance settings
    public AppearanceSettingsDto Appearance { get; set; } = new();

    // Playback settings  
    public PlaybackSettingsDto Playback { get; set; } = new();

    // Notification settings
    public NotificationSettingsDto Notifications { get; set; } = new();

    // Can add ANY new settings without database migration!
    public Dictionary<string, object> CustomSettings { get; set; } = [];
}
