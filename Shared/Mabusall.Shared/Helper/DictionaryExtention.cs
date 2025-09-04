namespace Mabusall.Shared.Helper;

public static class DictionaryExtention
{
    public static Dictionary<string, object> ToQueryDictionary<T>(this T obj, bool ignoreJsonPropertyName = false)
    {
        var dictionary = new Dictionary<string, object>();

        foreach (PropertyInfo p in typeof(T).GetProperties())
        {
            var jsonProperty = ignoreJsonPropertyName ? null : p.GetCustomAttribute<JsonPropertyNameAttribute>();

            string propertyName = jsonProperty?.Name;
            if (jsonProperty is null)
                propertyName = p.Name;

            var value = p.GetValue(obj, null);
            if (value is not null)
                dictionary.Add(propertyName, p.GetValue(obj, null));
        }

        return dictionary;
    }

    public static Dictionary<string, string> ToFormUrlDictionary<T>(this T obj, bool ignoreJsonPropertyName = false)
    {
        var dictionary = new Dictionary<string, string>();

        foreach (PropertyInfo p in typeof(T).GetProperties())
        {
            var jsonProperty = ignoreJsonPropertyName ? null : p.GetCustomAttribute<JsonPropertyNameAttribute>();

            string propertyName = jsonProperty?.Name;
            if (jsonProperty is null)
                propertyName = p.Name;

            var value = p.GetValue(obj, null);
            if (value is not null)
                dictionary.Add(propertyName, p.GetValue(obj, null).ToString());
        }

        return dictionary;
    }
}
