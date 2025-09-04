namespace Tasheer.Notification.Helper;

public static class JsonElemntHelper
{
    public static string? GetPropertyValue(this JsonElement jsonElement, string propertyName)
    {
        if (jsonElement.TryGetProperty(propertyName, out JsonElement propertyElement))
        {
            return propertyElement.GetString();
        }

        // Return null if the property does not exist or is not a string
        return null;
    }
}
