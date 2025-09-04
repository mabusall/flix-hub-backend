namespace Mabusall.Notification.Helper;

public static class JsonElemntHelper
{
    public static string? GetPropertyValue(this JsonElement jsonElement, string propertyName)
    {
        try
        {
            if (jsonElement.TryGetProperty(propertyName, out JsonElement propertyElement))
            {
                if (propertyElement.ValueKind == JsonValueKind.String)
                    return propertyElement.GetString();

                if (propertyElement.ValueKind == JsonValueKind.Number)
                    return propertyElement.GetInt32().ToString();
            }
        }
        catch { }

        // Return null if the property does not exist or is not a string
        return null;
    }
}