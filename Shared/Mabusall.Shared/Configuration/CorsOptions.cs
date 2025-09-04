namespace Tasheer.Shared.Configuration;

public class CorsOptions
{
    required public bool Enabled { get; set; }
    required public bool AllowAnyOrigin { get; set; }
    required public List<string> Origins { get; set; } = [];
}