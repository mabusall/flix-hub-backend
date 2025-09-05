namespace FlixHub.Shared.Helper;

public static class JsonSerializerHandler
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static string Serialize<TValue>(this TValue value, JsonSerializerOptions options = null)
    {
        if (value is null) return null;

        return JsonSerializer.Serialize(value, options ?? _options);
    }

    public static TValue Deserialize<TValue>(this string json, JsonSerializerOptions options = null)
    {
        if (string.IsNullOrWhiteSpace(json)) return default;

        if (typeof(TValue).Equals(typeof(string)))
            return (TValue)Convert.ChangeType(json, typeof(TValue));

        return JsonSerializer.Deserialize<TValue>(json, options ?? _options);
    }
}