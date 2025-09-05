namespace FlixHub.Core.Api.Model;

public record PlaybackSettingsDto
{
    public string DefaultQuality { get; set; } = "1080p";
    public bool AutoplayNext { get; set; } = true;
    public bool SkipIntro { get; set; } = false;
}
