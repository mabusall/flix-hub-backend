namespace Mabusall.Shared.Helper;

public static class ObjectExtention
{
    /// <summary>
    /// Convert an object to a Byte Array.
    /// </summary>
    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;

        using var stream = new MemoryStream();

        JsonSerializer.Serialize(stream, obj, GetJsonSerializerOptions());

        return stream.ToArray();
    }

    /// <summary>
    /// Convert a byte array to an Object of T.
    /// </summary>
    public static T ByteArrayToObject<T>(byte[] arrBytes)
    {
        using var stream = new MemoryStream();

        // Ensure that our stream is at the beginning.
        stream.Write(arrBytes, 0, arrBytes.Length);
        stream.Seek(0, SeekOrigin.Begin);

        return JsonSerializer.Deserialize<T>(stream, GetJsonSerializerOptions());
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }

    public static async Task<StringContent> ToStringContent<T>(this T obj, CancellationToken cancellationToken)
        => new(
            content: await JsonContent.Create(obj).ReadAsStringAsync(cancellationToken),
            encoding: Encoding.UTF8,
            mediaType: MediaTypeNames.Application.Json
        );

    public static IEnumerable<PropertyInfo> GetProperties(this object obj, Type type)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(type);

        return obj
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == type && p.CanRead && p.CanWrite);
    }

    public static string GenerateTemporaryJobKey(this string identifier, string module)
        => $"{module}_{identifier}";

    public static string GenerateTemporaryJobKey(this int referenceId, string module, string identifier)
        => $"{module}_{identifier}_{referenceId}";

    public static string GenerateTemporaryJobKey(this Guid referenceId, string module, string identifier)
        => $"{module}_{identifier}_{referenceId:N}";

    public static string ToCronExpression(this TimeSpan timeSpan)
    {
        // Ensure TimeSpan is valid
        if (timeSpan.TotalDays >= 365)
            timeSpan = new TimeSpan(1, 0, 0, 0); // change to 1 day

        // Extract days, hours, minutes, and seconds
        string minuteField = timeSpan.Minutes > 0 ? $"*/{timeSpan.Minutes}" : "0";
        string hourField = timeSpan.Hours > 0 ? $"*/{timeSpan.Hours}" : "*";
        string dayField = timeSpan.Days > 0 ? $"*/{timeSpan.Days}" : "*";

        // Generate cron: minute, hour, day, month, weekday
        return $"{minuteField} {hourField} {dayField} * *";
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            throw new ArgumentException("Order by string cannot be null or empty.", nameof(orderBy));

        // Split into property and direction
        var parts = orderBy.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 1 || parts.Length > 2)
            throw new ArgumentException("Order by string must be in the format 'Property Direction'.", nameof(orderBy));

        var propertyName = parts[0];
        var direction = parts.Length == 2 ? parts[1].ToLower() : "asc";

        if (direction != "asc" && direction != "desc")
            throw new ArgumentException("Order direction must be 'asc' or 'desc'.", nameof(orderBy));

        // Build parameter for lambda (x => x.PropertyName)
        var parameter = Expression.Parameter(typeof(T), "x");

        // Handle nested properties (e.g., "Parent.Child.Property")
        Expression propertyAccess = parameter;
        foreach (var property in propertyName.Split('.'))
        {
            propertyAccess = Expression.PropertyOrField(propertyAccess, property);
        }

        // Create the lambda expression for the selector
        var selector = Expression.Lambda(propertyAccess, parameter);

        // Determine the correct method to call
        var methodName = direction == "asc" ? "OrderBy" : "OrderByDescending";

        // Build the method call dynamically
        var methodCallExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(T), propertyAccess.Type],
            source.Expression,
            Expression.Quote(selector)
        );

        // Return the queryable with the applied ordering
        return source.Provider.CreateQuery<T>(methodCallExpression);
    }
}