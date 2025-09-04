namespace Mabusall.Shared.Helper;

public static class DateTimeExtention
{
    public static DateTime ToDateTime(this DateOnly value)
       => value.ToDateTime(TimeOnly.MinValue);

    public static DateTime ToDateTime(this TimeOnly value)
        => DateTime.MinValue + value.ToTimeSpan();

    public static DateOnly ToDateOnly(this DateTime value)
        => DateOnly.FromDateTime(value);

    public static TimeOnly ToTimeOnly(this DateTime value)
        => TimeOnly.FromDateTime(value);

    public static DateTime ParseElmDate(this string value)
    {
        if (DateTime.TryParse(value, out DateTime dateTime))
            return dateTime;

        var strings = value.Split('+');

        if (strings.Length > 1 && DateTime.TryParse(strings[0], out dateTime))
            return dateTime;

        throw new Exception($"Invalid date format {value}");
    }

    public static string ToViewString(this DateTime date, bool isArabic, string formatString)
    {
        // Define the cultures
        CultureInfo arabicCulture = new("ar-SA");
        CultureInfo englishCulture = new("en-US");

        // Select the appropriate culture
        CultureInfo selectedCulture = isArabic ? arabicCulture : englishCulture;

        // Ensure Gregorian calendar for Arabic
        if (isArabic)
        {
            selectedCulture.DateTimeFormat.Calendar = new GregorianCalendar();
        }

        // Format the date
        return date.ToString(formatString, selectedCulture);
    }
}
