namespace FlixHub.Core.Api.Model;

public record AppearanceSettingsDto
{
    public string VideoQuality { get; set; } = "auto";
    public bool AutoplayNext { get; set; } = true;
    public bool SkipIntro { get; set; } = false;
}
