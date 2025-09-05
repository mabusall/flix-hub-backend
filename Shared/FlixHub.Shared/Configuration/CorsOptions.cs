namespace FlixHub.Shared.Configuration;

public class CorsOptions
{
    public required bool Enabled { get; set; }
    public required bool AllowAnyOrigin { get; set; }
    public required List<string> Origins { get; set; } = [];
}